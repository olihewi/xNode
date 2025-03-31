using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace XNode.Flow
{
    public class FlowGraphPlayer : MonoBehaviour
    {
        public static readonly List<FlowGraphPlayer> LoadedPlayers = new(1);
        public static readonly List<FlowGraphPlayer> ActivePlayers = new(1);

        public FlowGraph flowGraph;
        public bool playOnAwake = true;

        public HashSet<FlowContext> FlowContexts { get; private set; } = new();
        public List<(FlowContext ctx, FlowCancellationFlags cancellationFlags)> CancelledContexts { get; private set; } = new();
        public List<FlowContext> QueuedContexts { get; private set; } = new();

        public delegate void ExecutionAction(FlowContext ctx);
        public event ExecutionAction OnStartExecution, OnEndExecution;


        private void Update()
        {
        #if UNITY_EDITOR
            if (!Application.isPlaying) return;
        #endif
            PerformExecution();
            PerformCancellation();
        }

        public void Execute(FlowContext ctx) => QueuedContexts.Add(ctx);
        public void Execute(IEnumerable<FlowContext> ctxs) => QueuedContexts.AddRange(ctxs);

        private void PerformExecution()
        {
            if (QueuedContexts.Count == 0) return;
            for (int i = 0; i < QueuedContexts.Count; i++)
            {
                var ctx = QueuedContexts[i];
                if (ctx == null || ctx.Node == null) return;
                switch (ctx.Node)
                {
                case FlowNode flowNode:
                {
                    OnStartExecution?.Invoke(ctx);
                    flowNode.Perform(ctx);
                    flowNode.OnFinishedPerforming(ctx);
                    OnEndExecution?.Invoke(ctx);
                    break;
                }
                case FlowRoutineNode routineNode:
                {
                    ctx.Coroutine = StartCoroutine(ExecuteRoutine(routineNode, ctx));
                    break;
                }
                }
            }
            QueuedContexts.Clear();
        }

        private void PerformCancellation()
        {
            if (CancelledContexts.Count == 0) return;
            for (int i = 0; i < CancelledContexts.Count; i++)
            {
                var ctx = CancelledContexts[i].ctx;
                if (ctx == null || ctx.Node == null) return;
                if (ctx.Coroutine != null) StopCoroutine(ctx.Coroutine);
                if (ctx.Node is FlowRoutineNode routineNode) routineNode.OnCancelled(ctx, CancelledContexts[i].cancellationFlags);
            }
            CancelledContexts.Clear();
        }

        private IEnumerator ExecuteRoutine(FlowRoutineNode routineNode, FlowContext ctx)
        {
            FlowContexts.Add(ctx);
            OnStartExecution?.Invoke(ctx);
            yield return routineNode.Perform(ctx);
            FlowContexts.Remove(ctx);
            OnEndExecution?.Invoke(ctx);
        }

        public void CancelExecution(FlowContext ctx,
            FlowCancellationFlags cancellationFlags = FlowCancellationFlags.None)
        {
            ctx.Cancel();
        }

    }
}
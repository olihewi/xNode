using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using XNode.Variables;

namespace XNode.Flow
{
    public class FlowGraphPlayer : MonoBehaviour, INodeVariableProvider
    {
        public FlowGraph flowGraph;
        public bool playOnAwake = true;

        public HashSet<FlowContext> FlowContexts { get; private set; } = new();
        public List<(FlowContext ctx, FlowCancellationFlags cancellationFlags)> CancelledContexts { get; private set; } = new();
        public List<FlowContext> QueuedContexts { get; private set; } = new();

        public delegate void ExecutionAction(FlowContext ctx);
        public event ExecutionAction OnStartExecution, OnEndExecution;


        private void Awake()
        {
        #if UNITY_EDITOR
            if (!Application.isPlaying) return;
        #endif
            if (flowGraph == null) return;
            if (playOnAwake && flowGraph.EntryNode != null)
            {
                Execute(FlowContext.EntryContext(flowGraph.EntryNode, this));
            }
        }

        private void Update()
        {
        #if UNITY_EDITOR
            if (!Application.isPlaying) return;
        #endif
            PerformCancellation();
        }

        public void Execute(FlowContext ctx)
        {
            QueuedContexts.Add(ctx);
            if (!_isExecuting) PerformExecution();
        }

        public void Execute(IEnumerable<FlowContext> ctxs)
        {
            QueuedContexts.AddRange(ctxs);
            if (!_isExecuting) PerformExecution();
        }

        private bool _isExecuting = false;
        private void PerformExecution()
        {
            if (QueuedContexts.Count == 0) return;
            _isExecuting = true;
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
            _isExecuting = false;
        }
        private IEnumerator ExecuteRoutine(FlowRoutineNode routineNode, FlowContext ctx)
        {
            FlowContexts.Add(ctx);
            OnStartExecution?.Invoke(ctx);
            yield return routineNode.Perform(ctx);
            routineNode.OnFinishedPerforming(ctx);
            FlowContexts.Remove(ctx);
            OnEndExecution?.Invoke(ctx);
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

        public void CancelExecution(FlowContext ctx,
            FlowCancellationFlags cancellationFlags = FlowCancellationFlags.None)
        {
            ctx.Cancel();
        }

        public Dictionary<string, object> NodeVariables { get; set; }
    }
}
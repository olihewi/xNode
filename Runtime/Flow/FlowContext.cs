using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XNode.Flow
{
    public class FlowContext
    {
        public FlowGraphPlayer Player { get; set; }
        public BaseFlowNode Node { get; private set; }
        public NodePort EntryPort { get; private set; }
        public NodePort PreviousExitPort { get; private set; }
        public float StartTime { get; private set; }
        public Coroutine Coroutine { get; set; }

        /// <summary>
        /// Used for subgraph execution handling.
        /// </summary>
        public FlowContext ParentGraphContext { get; private set; }



        public BaseFlowNode PreviousNode => PreviousExitPort?.node as BaseFlowNode;
        public FlowGraph MasterGraph
        {
            get
            {
                var parentCtx = this;
                while (parentCtx.ParentGraphContext != null)
                {
                    parentCtx = parentCtx.ParentGraphContext;
                }
                return parentCtx.Node.graph as FlowGraph;
            }
        }

        public FlowContext(BaseFlowNode node, NodePort exitPort, NodePort entryPort, FlowContext prevCtx)
        {
            Node = node;
            EntryPort = entryPort;
            PreviousExitPort = exitPort;
            if (prevCtx != null)
            {
                Player = prevCtx.Player;
                ParentGraphContext = prevCtx.ParentGraphContext;
            }
            else
            {
                Player = null;
                ParentGraphContext = null;
            }
            StartTime = Time.realtimeSinceStartup;
        }

        public static FlowContext EntryContext(BaseFlowNode node, FlowGraphPlayer player, FlowContext parentCtx = null)
        {
            return new FlowContext(node, null, null, null) { Player = player, ParentGraphContext = parentCtx };
        }

        public static IEnumerable<FlowContext> FromExitPort(NodePort exitPort, FlowContext prevCtx)
        {
            return exitPort
                .GetConnections()
                .Select(entryPort => new FlowContext(entryPort.node as BaseFlowNode, exitPort, entryPort, prevCtx));
        }

        public static IEnumerable<FlowContext> FromExitPort(BaseFlowNode exitNode, string exitPortName,
            FlowContext prevCtx)
        {
            return FromExitPort(exitNode.GetOutputPort(exitPortName), prevCtx);
        }

        public void Cancel(FlowCancellationFlags cancellationFlags = FlowCancellationFlags.None)
        {
            Player.CancelledContexts.Add((this, cancellationFlags));
        }
    }

    [Flags]
    public enum FlowCancellationFlags
    {
        None = 0,
        CompleteExecution = 1 << 0,
    }
}
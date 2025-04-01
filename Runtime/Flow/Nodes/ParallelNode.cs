using UnityEngine;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Parallel")]
    [NodeWidth(100), NodeTint(0.5F, 0.1F, 0.1F)]
    public class ParallelNode : InFlowNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode nextNode;
        public override void Perform(FlowContext ctx) { }

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(nameof(nextNode), ctx);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (from.node == this && from.fieldName == nameof(nextNode)) nextNode = to.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(nextNode)) nextNode = null;
        }
    }
}
using System.Collections;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Cancel Execution")]
    [NodeWidth(128), NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public class CancelExecutionNode : InOutFlowRoutineNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        public BaseFlowNode cancel;

        public override IEnumerator Perform(FlowContext ctx)
        {
            yield break;
        }
    }
}
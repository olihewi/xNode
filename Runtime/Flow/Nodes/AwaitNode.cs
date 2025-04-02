using System.Collections;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Await")]
    [NodeWidth(100), NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public class AwaitNode : InOutFlowRoutineNode
    {
        public override IEnumerator Perform(FlowContext ctx)
        {
            yield break;
        }
    }
}
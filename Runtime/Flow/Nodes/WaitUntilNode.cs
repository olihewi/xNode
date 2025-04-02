using System.Collections;
using UnityEngine;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Wait Until")]
    [NodeWidth(160), NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public class WaitUntilNode : InOutFlowRoutineNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public bool condition;
        public override IEnumerator Perform(FlowContext ctx)
        {
            yield return new WaitUntil(() => GetInputValue(nameof(condition), condition, ctx));
        }
    }
}
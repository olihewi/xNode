using System.Collections;
using UnityEngine;
using XNode.Flow;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Wait For Seconds")]
    [NodeWidth(160), NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public class WaitForSecondsNode : InOutFlowRoutineNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public float duration = 1F;
        public bool unscaled = false;
        public override IEnumerator Perform(FlowContext ctx)
        {
            if (unscaled) yield return new WaitForSecondsRealtime(GetInputValue(nameof(duration), duration, ctx));
            else yield return new WaitForSeconds(GetInputValue(nameof(duration), duration, ctx));
        }
    }
}
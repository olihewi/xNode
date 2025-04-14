using UnityEngine;

namespace XNode.Flow.Nodes
{
    public class DebugLogNode : InOutFlowNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public string logMessage;

        public override void Perform(FlowContext ctx)
        {
            Debug.Log(GetInputValue(nameof(logMessage), logMessage, ctx), this);
        }
    }
}
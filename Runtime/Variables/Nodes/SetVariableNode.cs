using System;
using XNode.Flow;

namespace XNode.Variables
{
    [CreateNodeMenu("Variables/Set Variable", 1)]
    public class SetVariableNode : InOutFlowNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public string Key;

        public VariableScope Scope = VariableScope.FlowGraphPlayer;

        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)]
        public NodeObjectWrapper Value;

        public override void Perform(FlowContext ctx)
        {
            var key = GetInputValue(nameof(Key), Key, ctx);
            var value = GetInputValue<object>(nameof(Value), null, ctx);
            SetVariable(key, value, Scope, ctx);
        }
    }
}
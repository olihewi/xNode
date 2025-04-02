using XNode.Flow;

namespace XNode.Variables
{
    [CreateNodeMenu("Variables/Get Variable", 0)]
    public class GetVariableNode : Node
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public string Key;

        public VariableScopeFlags Scopes = VariableScopeFlags.All;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None)]
        public NodeObjectWrapper Value;

        public override object GetValue(NodePort port, FlowContext ctx = null)
        {
            var key = GetInputValue(nameof(Key), Key, ctx);
            return GetVariable(key, Scopes, ctx);
        }
    }
}
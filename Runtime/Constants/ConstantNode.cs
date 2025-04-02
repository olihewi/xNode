using XNode.Flow;

namespace XNode.Nodes
{
    [NodeWidth(160)]
    public abstract class ConstantNode : Node {}
    public abstract class ConstantNode<TValue> : ConstantNode
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.None)]
        public TValue Value;

        public override object GetValue(NodePort port, FlowContext ctx = null)
        {
            return Value;
        }
    }
}
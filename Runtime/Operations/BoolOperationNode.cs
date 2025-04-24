using System;
using XNode.Flow;

namespace XNode.Operations
{
    [NodeTypeTint(typeof(bool))]
    [CreateNodeMenu("Operations/Bool Operation")]
    public class BoolOperationNode : Node
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
        public bool first;

        public enum Operation
        {
            Not = 10,
            And = 20,
            Or = 30,
            Xor = 40,
            Nor = 50,
            Nand = 60,
            XNor = 70,
        }

        public Operation operation = Operation.And;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
        public bool second;

        [Output] public bool result;

        public override object GetValue(NodePort port, FlowContext ctx = null)
        {
            var first = GetInputValue(nameof(this.first), this.first, ctx);
            var second = GetInputValue(nameof(this.second), this.second, ctx);
            return operation switch
            {
                Operation.Not => !first,
                Operation.And => first && second,
                Operation.Or => first || second,
                Operation.Xor => first ^ second,
                Operation.Nor => !(first || second),
                Operation.Nand => !(first && second),
                Operation.XNor => !(first ^ second),
                _ => false,
            };
        }
    }
}
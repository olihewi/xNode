using System;
using XNode.Flow;
using Object = UnityEngine.Object;

namespace XNode.Operations.Comparison
{
    [NodeTypeTint(typeof(Object))]
    [CreateNodeMenu("Operations/Comparison/Object Comparison")]
    public class ObjectComparisonNode : Node
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public Object first;

        public enum Comparison
        {
            EqualTo = 10,
            NotEqualTo = 20,
        }
        public Comparison comparison = Comparison.EqualTo;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public Object second;

        [Output] public bool result;

        public override object GetValue(NodePort port, FlowContext ctx = null)
        {
            var first = GetInputValue(nameof(this.first), this.first, ctx);
            var second = GetInputValue(nameof(this.second), this.second, ctx);
            return comparison switch
            {
                Comparison.EqualTo => first == second,
                Comparison.NotEqualTo => first != second,
                _ => false
            };
        }
    }
}
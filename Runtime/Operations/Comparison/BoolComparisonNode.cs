using XNode.Flow;

namespace XNode.Operations.Comparison
{
    [NodeTypeTint(typeof(bool))]
    [CreateNodeMenu("Operations/Comparison/Bool Comparison")]
    public class BoolComparisonNode : Node
    {

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public bool first;
        public enum Comparison
        {
            EqualTo = 10,
            NotEqualTo = 20,
        }
        public Comparison comparison = Comparison.EqualTo;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public bool second;

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
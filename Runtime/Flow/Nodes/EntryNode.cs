namespace XNode.Flow.Nodes
{
    [NodeTypeTint(typeof(BaseFlowNode))]
    [CreateNodeMenu("Execution/Entry", -102)]
    [DisallowMultipleNodes]
    public class EntryNode : OutFlowNode
    {
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.None, true)]
        public NodeObjectWrapper[] inputs;
        public override void Perform(FlowContext ctx)
        {
        }

        public override object GetValue(NodePort port, FlowContext ctx)
        {
            if (!port.fieldName.StartsWith(nameof(inputs))) return base.GetValue(port, ctx);

            if (int.TryParse(port.fieldName[(nameof(inputs).Length + 1)..], out int idx) && idx >= 0 &&
                idx < inputs.Length)
            {
                return inputs[idx].Value;
            }

            return null;
        }
    }
}
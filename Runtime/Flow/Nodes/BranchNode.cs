namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Branch")]
    [NodeWidth(128), NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public class BranchNode : InFlowNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public bool condition;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public BaseFlowNode branchTrue;
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public BaseFlowNode branchFalse;

        public override void Perform(FlowContext ctx)
        {
        }

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(GetInputValue(nameof(condition), condition, ctx) ? nameof(branchTrue) : nameof(branchFalse), ctx);
        }
    }
}
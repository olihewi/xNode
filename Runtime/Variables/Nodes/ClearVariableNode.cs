using XNode.Flow;

namespace XNode.Variables
{
    [CreateNodeMenu("Variables/Clear Variable", 2)]
    public class ClearVariableNode : InOutFlowNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public string Key;

        public VariableScopeFlags Scopes = VariableScopeFlags.All;

        public override void Perform(FlowContext ctx)
        {
            var keyPort = GetInputPort(nameof(Key));
            if (keyPort.IsConnected)
            {
                if (!keyPort.TryGetInputValue(out string key)) return;

                if (Scopes.HasFlag(VariableScopeFlags.FlowContext))
                    ctx.ClearVariable(key);

                if (Scopes.HasFlag(VariableScopeFlags.Graph) && graph is INodeVariableProvider variableGraph)
                    variableGraph.ClearVariable(key);

                if (Scopes.HasFlag(VariableScopeFlags.ParentGraphs))
                {
                    var parentCtx = ctx.ParentGraphContext;
                    while (parentCtx != null && parentCtx.Node != null)
                    {
                        if (parentCtx.Node.graph is INodeVariableProvider parentVariableGraph)
                            parentVariableGraph.ClearVariable(key);
                        parentCtx = parentCtx.ParentGraphContext;
                    }
                }

                if (Scopes.HasFlag(VariableScopeFlags.FlowGraphPlayer))
                    ctx.Player.ClearVariable(key);

                if (Scopes.HasFlag(VariableScopeFlags.Global))
                    GlobalVariables.ClearVariable(key);
            }
            else
            {
                if (Scopes.HasFlag(VariableScopeFlags.FlowContext))
                    ctx.ClearAllVariables();

                if (Scopes.HasFlag(VariableScopeFlags.Graph) && graph is INodeVariableProvider variableGraph)
                    variableGraph.ClearAllVariables();

                if (Scopes.HasFlag(VariableScopeFlags.ParentGraphs))
                {
                    var parentCtx = ctx.ParentGraphContext;
                    while (parentCtx != null && parentCtx.Node != null)
                    {
                        if (parentCtx.Node.graph is INodeVariableProvider parentVariableGraph)
                            parentVariableGraph.ClearAllVariables();
                        parentCtx = parentCtx.ParentGraphContext;
                    }
                }

                if (Scopes.HasFlag(VariableScopeFlags.FlowGraphPlayer))
                    ctx.Player.ClearAllVariables();

                if (Scopes.HasFlag(VariableScopeFlags.Global))
                    GlobalVariables.ClearAllVariables();
            }
        }
    }
}
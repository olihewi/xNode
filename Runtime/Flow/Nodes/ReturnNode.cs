using UnityEngine;

namespace XNode.Flow.Nodes
{
    [NodeTypeTint(typeof(BaseFlowNode))]
    [CreateNodeMenu("Execution/Return", -101)]
    [DisallowMultipleNodes]
    public class ReturnNode : InFlowNode
    {
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.None, true)]
        public NodeObjectWrapper[] outputs;
        public override void Perform(FlowContext ctx)
        {
            if (ctx.ParentGraphContext == null) return;
            if (ctx.ParentGraphContext.Node is SubgraphNode subgraphNode)
            {
                var length = Mathf.Min(outputs.Length, subgraphNode.outputs.Length);
                for (int i = 0; i < length; i++)
                {
                    var port = GetInputPort($"{nameof(outputs)} {i}");
                    if (port == null) continue;
                    subgraphNode.outputs[i].Value = port.GetInputValue();
                }
            }
            ctx.ParentGraphContext.Cancel(FlowCancellationFlags.CompleteExecution);

        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace XNode.Flow.Nodes
{
    [CreateNodeMenu("Execution/Subgraph", -100)]
    [NodeTypeTint(typeof(BaseFlowNode)), NodeWidth(256)]
    public class SubgraphNode : FlowRoutineNode
    {
        public NodeGraph subgraph;

        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] public BaseFlowNode enter;
        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.None, true)]
        public NodeObjectWrapper[] inputs;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField] public BaseFlowNode exit;
        [Output(ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.None, true)]
        public NodeObjectWrapper[] outputs;

        [SerializeField, HideInInspector] private EntryNode subgraphEntryNode;
        [SerializeField, HideInInspector] private ReturnNode subgraphReturnNode;

        public override IEnumerator Perform(FlowContext ctx)
        {
            if (subgraph == null)
            {
                Debug.LogWarning($"{this} on {graph} @ {position}: No subgraph assigned!", this);
                yield break;
            }

            if (subgraphEntryNode == null)
            {
                Debug.LogWarning($"{this} on {graph} @ {position}: Missing entry node on subgraph!", this);
                yield break;
            }

            var inputLength = Mathf.Min(inputs.Length, subgraphEntryNode.inputs.Length);
            for (int i = 0; i < inputLength; i++)
            {
                var port = GetInputPort($"{nameof(inputs)} {i}");
                if (port == null) continue;
                subgraphEntryNode.inputs[i].Value = port.GetInputValue(ctx);
            }
            ctx.Player.Execute(FlowContext.EntryContext(subgraphEntryNode, ctx.Player, ctx));
            yield return new WaitUntil(() => false);
        }

        public override void OnCancelled(FlowContext ctx, FlowCancellationFlags cancellationFlags)
        {
            if (cancellationFlags.HasFlag(FlowCancellationFlags.CompleteExecution)) ExecuteOutput(nameof(exit), ctx);
        }

        public override object GetValue(NodePort port, FlowContext ctx)
        {
            if (port.fieldName.StartsWith(nameof(outputs)))
            {
                if (int.TryParse(port.fieldName[(nameof(outputs).Length + 1)..], out int idx) && idx >= 0 &&
                    idx < outputs.Length)
                {
                    return outputs[idx].Value;
                }
            }
            return base.GetValue(port, ctx);
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            if (subgraph == null) return;

            foreach (var node in subgraph.nodes)
            {
                if (node is EntryNode entryNode)
                {
                    subgraphEntryNode = entryNode;
                }
                else if (node is ReturnNode returnNode)
                {
                    subgraphReturnNode = returnNode;
                }
            }

            if (subgraphEntryNode != null)
            {
                if (subgraphEntryNode.inputs.Length != inputs.Length)
                {
                    for (int i = inputs.Length - 1; i >= subgraphEntryNode.inputs.Length; i--)
                    {
                        RemoveDynamicPort($"{nameof(inputs)} {i}");
                    }
                    for (int i = inputs.Length; i < subgraphEntryNode.inputs.Length; i++)
                    {
                        AddDynamicInput(typeof(NodeObjectWrapper), ConnectionType.Override, TypeConstraint.None,
                            $"{nameof(inputs)} {i}");
                    }
                    Array.Resize(ref inputs, subgraphEntryNode.inputs.Length);
                }
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i].name = subgraphEntryNode.inputs[i].name;
                }
            }
            else if (inputs.Length != 0)
            {
                for (int i = inputs.Length - 1; i >= 0; i--)
                {
                    RemoveDynamicPort($"{nameof(inputs)} {i}");
                }
                Array.Resize(ref inputs, 0);
            }

            if (subgraphReturnNode != null)
            {
                if (subgraphReturnNode.outputs.Length != outputs.Length)
                {
                    for (int i = outputs.Length - 1; i >= subgraphReturnNode.outputs.Length; i--)
                    {
                        RemoveDynamicPort($"{nameof(outputs)} {i}");
                    }
                    for (int i = outputs.Length; i < subgraphReturnNode.outputs.Length; i++)
                    {
                        AddDynamicOutput(typeof(NodeObjectWrapper), ConnectionType.Multiple, TypeConstraint.None,
                            $"{nameof(outputs)} {i}");
                    }
                    Array.Resize(ref outputs, subgraphReturnNode.outputs.Length);
                }
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i].name = subgraphReturnNode.outputs[i].name;
                }
            }
            else if (outputs.Length != 0)
            {
                for (int i = outputs.Length - 1; i >= 0; i--)
                {
                    RemoveDynamicPort($"{nameof(outputs)} {i}");
                }
                Array.Resize(ref outputs, 0);
            }
        }
    #endif
    }
}
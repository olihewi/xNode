using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XNode.Flow.Nodes
{
    [NodeTypeTint(typeof(BaseFlowNode), 0.5F)]
    public abstract class SwitchNode : InFlowNode {}
    public abstract class SwitchNode<T> : SwitchNode
    {
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)]
        public T value;

        [Serializable]
        public struct Branch
        {
            public string key;
            public T value;
        }
        public Branch[] branches;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)]
        public BaseFlowNode defaultSwitch;

        public override void Perform(FlowContext ctx)
        {
        }

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            var input = GetInputValue(nameof(value), value, ctx);
            foreach (var branch in branches)
            {
                if (EqualityComparer<T>.Default.Equals(branch.value, input))
                {
                    ExecuteOutput(branch.key, ctx);
                    return;
                }
            }
            ExecuteOutput(nameof(defaultSwitch), ctx);
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            var seenGuids = new HashSet<string>();
            for (int i = 0; i < branches.Length; i++)
            {
                ref var branch = ref branches[i];
                if (string.IsNullOrEmpty(branch.key) || seenGuids.Contains(branch.key))
                {
                    branch.key = Guid.NewGuid().ToString();
                }

                if (GetOutputPort(branch.key) == null)
                {
                    AddDynamicOutput(typeof(BaseFlowNode), ConnectionType.Override, TypeConstraint.Inherited,
                        branch.key);
                }
                seenGuids.Add(branch.key);
            }
            var portsToRemove = DynamicPorts.Where(x => branches.All(y => y.key != x.fieldName)).Select(x => x.fieldName).ToArray();
            foreach (var portToRemove in portsToRemove)
            {
                RemoveDynamicPort(portToRemove);
            }

        }
    #endif
    }
}
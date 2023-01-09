using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.Flow
{
    [Serializable]
    public abstract class FlowNode : Node
    {
        public override object GetValue(NodePort port)
        {
            if (port.IsConnected && port.Connection.node is FlowNode flowNode) return flowNode;
            return null;
        }

        public virtual IEnumerator Perform()
        {
            yield break;
        }

        public virtual IEnumerable<FlowNode> PreviousNodes => Array.Empty<FlowNode>();
        public virtual IEnumerable<FlowNode> NextNodes => Array.Empty<FlowNode>();

        public FlowNode GetFlowNode(string portName) => GetValue(GetPort(portName)) as FlowNode;
    }

    [Serializable]
    public abstract class InFlowNode : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        [SerializeField, HideInInspector]
        public FlowNode previousNode;

        public override IEnumerable<FlowNode> PreviousNodes
        {
            get { yield return GetFlowNode("previousNodes"); }
        }
    }

    [Serializable]
    public abstract class OutFlowNode : FlowNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        [SerializeField, HideInInspector]
        public FlowNode nextNode;

        public override IEnumerable<FlowNode> NextNodes
        {
            get { yield return GetFlowNode("nextNode"); }
        }
    }

    [Serializable]
    public abstract class InOutFlowNode : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        [SerializeField, HideInInspector]
        public FlowNode previousNode;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
        [SerializeField, HideInInspector]
        public FlowNode nextNode;

        public override IEnumerable<FlowNode> PreviousNodes
        {
            get { yield return GetFlowNode("previousNodes"); }
        }

        public override IEnumerable<FlowNode> NextNodes
        {
            get { yield return GetFlowNode("nextNode"); }
        }
    }
}
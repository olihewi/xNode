using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace XNode.Flow
{
    public abstract class BaseFlowNode : Node
    {
        public virtual void OnFinishedPerforming(FlowContext ctx)
        {
        }

        protected void ExecuteOutput(NodePort exitPort, FlowContext ctx)
        {
            if (ctx.Player == null || exitPort == null) return;
            ctx.Player.Execute(FlowContext.FromExitPort(exitPort, ctx));
        }

        protected void ExecuteOutput(string exitPortName, FlowContext ctx)
        {
            ExecuteOutput(GetOutputPort(exitPortName), ctx);
        }

        public override object GetValue(NodePort port, FlowContext ctx)
        {
            if (port.IsConnected && port.Connection.node is BaseFlowNode flowNode) return flowNode;
            return null;
        }
    }

    public abstract class FlowNode : BaseFlowNode
    {
        public abstract void Perform(FlowContext ctx);
    }

    public abstract class FlowRoutineNode : BaseFlowNode
    {
        public abstract IEnumerator Perform(FlowContext ctx);
        public virtual void OnCancelled(FlowContext ctx, FlowCancellationFlags cancellationFlags) {}
    }

#region Template Nodes

    public abstract class InFlowNode : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode previousNode;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (to.node == this && to.fieldName == nameof(previousNode)) previousNode = from.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(previousNode)) previousNode = null;
        }
    }
    public abstract class OutFlowNode : FlowNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode nextNode;

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(nameof(nextNode), ctx);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (from.node == this && from.fieldName == nameof(nextNode)) nextNode = to.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(nextNode)) nextNode = null;
        }
    }

    public abstract class InOutFlowNode : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode previousNode;
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode nextNode;

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(nameof(nextNode), ctx);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (to.node == this && to.fieldName == nameof(previousNode)) previousNode = from.node as BaseFlowNode;
            else if (from.node == this && from.fieldName == nameof(nextNode)) nextNode = to.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            switch (port.fieldName)
            {
            case nameof(nextNode):
                nextNode = null;
                break;
            case nameof(previousNode):
                previousNode = null;
                break;
            }
        }
    }

    public abstract class InFlowRoutineNode : FlowRoutineNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode previousNode;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (to.node == this && to.fieldName == nameof(previousNode)) previousNode = from.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(previousNode)) previousNode = null;
        }
    }
    public abstract class OutFlowRoutineNode : FlowRoutineNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode nextNode;

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(nameof(nextNode), ctx);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (from.node == this && from.fieldName == nameof(nextNode)) nextNode = to.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            if (port.fieldName == nameof(nextNode)) nextNode = null;
        }
    }

    public abstract class InOutFlowRoutineNode : FlowRoutineNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode previousNode;
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited), SerializeField, HideInInspector]
        protected BaseFlowNode nextNode;

        public override void OnFinishedPerforming(FlowContext ctx)
        {
            ExecuteOutput(nameof(nextNode), ctx);
            base.OnFinishedPerforming(ctx);
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if (to.node == this && to.fieldName == nameof(previousNode)) previousNode = from.node as BaseFlowNode;
            else if (from.node == this && from.fieldName == nameof(nextNode)) nextNode = to.node as BaseFlowNode;
        }

        public override void OnRemoveConnection(NodePort port)
        {
            base.OnRemoveConnection(port);
            switch (port.fieldName)
            {
            case nameof(nextNode):
                nextNode = null;
                break;
            case nameof(previousNode):
                previousNode = null;
                break;
            }
        }
    }

#endregion
}
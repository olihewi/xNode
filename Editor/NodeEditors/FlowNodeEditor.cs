﻿
using XNode.Flow;

namespace XNodeEditor.NodeEditors
{
    [CustomNodeEditor(typeof(BaseFlowNode))]
    public class FlowNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.PortPair(target.GetPort("previousNode"), target.GetPort("nextNode"));
            base.OnBodyGUI();
        }
    }
}
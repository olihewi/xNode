using UnityEngine;
using XNode.Nodes;

namespace XNodeEditor.NodeEditors
{
    [CustomNodeEditor(typeof(ConstantNode))]
    public class ConstantNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("Value"), GUIContent.none);
        }
    }
}
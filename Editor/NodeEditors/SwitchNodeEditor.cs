using System;
using UnityEditor;
using UnityEngine;
using XNode;
using XNode.Flow;
using XNode.Flow.Nodes;

namespace XNodeEditor.NodeEditors
{
    [CustomNodeEditor(typeof(SwitchNode))]
    public class SwitchNodeEditor : FlowNodeEditor
    {
        public override bool ShowDynamicPorts => false;
    }

    [CustomPropertyDrawer(typeof(IntSwitchNode.Branch))]
    [CustomPropertyDrawer(typeof(StringSwitchNode.Branch))]
    [CustomPropertyDrawer(typeof(ObjectSwitchNode.Branch))]
    public class SwitchNodeBranchPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var portRect = new Rect(position) { xMin = position.xMax};
            var valueRect = new Rect(position) { xMax = portRect.xMin };
            var keyProp = property.FindPropertyRelative("key");
            var valueProp = property.FindPropertyRelative("value");

            EditorGUI.BeginChangeCheck();
            if (property.serializedObject.targetObject is Node node)
            {
                NodeEditorGUILayout.PortField(portRect.position, node.GetOutputPort(keyProp.stringValue));
            }
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
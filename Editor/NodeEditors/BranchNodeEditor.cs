using UnityEditor;
using UnityEngine;
using XNode.Flow.Nodes;

namespace XNodeEditor.NodeEditors
{
    [CustomNodeEditor(typeof(BranchNode))]
    public class BranchNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(target.GetInputPort("previousNode"), GUILayout.MinWidth(0));
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("branchTrue"), new GUIContent("True"), false, GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(new GUIContent("True"), target.GetOutputPort("branchTrue"), GUILayout.MinWidth(0));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("condition"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(new GUIContent("False"), target.GetOutputPort("branchFalse"), GUILayout.MinWidth(0));
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("branchFalse"), new GUIContent("False"), false, GUILayout.MinWidth(0));
            EditorGUILayout.EndHorizontal();
        }
    }
}
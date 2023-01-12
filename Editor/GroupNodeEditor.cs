using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;
using UnityEditor;

namespace XNodeEditor
{
    [CustomNodeEditor(typeof(XNode.GroupNode))]
    public class GroupNodeEditor : NodeEditor
    {
        private static Vector4 padding = new Vector4(32, 48, 32, 32);
        private bool _selected = false;
        private UnityEngine.Object[] lastSelection;

        public override void OnHeaderGUI()
        {
            if (target is XNode.GroupNode node)
            {
                bool selectChildren = NodeEditorWindow.currentActivity is NodeEditorWindow.NodeActivity.HoldNode or NodeEditorWindow.NodeActivity.DragNode;
                if (!_selected && selectChildren && Selection.objects.Contains(target))
                {
                    _selected = true;
                    var selection = Selection.objects.ToList();
                    lastSelection = selection.ToArray();
                    GetChildren(node, ref selection);
                    //selection.AddRange(GetChildren(node));
                    Selection.objects = selection.Distinct().ToArray();
                    NodeEditorWindow.current.Repaint();
                }
                else if (_selected && !selectChildren)
                {
                    _selected = false;
                    Selection.objects = lastSelection;
                }
            }
            base.OnHeaderGUI();
        }

        private void GetChildren(XNode.GroupNode group, ref List<UnityEngine.Object> list)
        {
            foreach (var child in group.children)
            {
                if (list.Contains(child)) continue;
                list.Add(child);
                if (child is XNode.GroupNode _group) GetChildren(_group, ref list);
            }
        }

        public override void OnBodyGUI()
        {
            var e = Event.current;
            var node = target as XNode.GroupNode;

            var nodes = node.graph.nodes;
            if (e.type == EventType.Repaint)
            {
                if (nodes.FirstOrDefault() != node)
                {
                    nodes.Remove(node);
                    int targetIndex = 0;
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i] is XNode.GroupNode _group)
                        {
                            if (_group.children.Contains(node))
                            {
                                targetIndex = i + 1;
                                break;
                            }
                        }
                        else break;
                    }
                    nodes.Insert(targetIndex, node);
                }
            }

            if (node == null || node.children == null || node.children.Count == 0) return;
            node.position = new Vector2(
                node.children.Min(x => x.position.x) - padding.x,
                node.children.Min(x => x.position.y) - padding.y
                );
            Debug.Log(GetSize(node.children.FirstOrDefault()));
            GUILayout.Label("");
        }

        public override Color GetTint()
        {
            Type type = target.GetType();
            if (type.TryGetAttributeTint(out Color color)) return color;

            if (!(target is XNode.GroupNode node)) return NodeEditorPreferences.GetSettings().tintColor;
            return node.color;
        }

        public override int GetWidth()
        {
            int min = base.GetWidth();
            var group = target as XNode.GroupNode;
            if (group == null || group.children == null || group.children.Count == 0) return min + (int)padding.x + (int)padding.z;
            return Mathf.Max(min,
                             group.children.Max(x =>
                                                   GetSize(x).x + (int)x.position.x) - (int)target.position.x)
                 + (int)padding.z;
        }

        public override int GetHeight()
        {
            int min = base.GetHeight();
            var group = target as XNode.GroupNode;
            if (group == null || group.children == null || group.children.Count == 0) return min + (int)padding.y + (int)padding.w;
            return Mathf.Max(min,
                             group.children.Max(x =>
                                                   GetSize(x).y + (int) x.position.y) - (int)target.position.y)
                 + (int) padding.w;
        }

        public Vector2Int GetSize(Node node) => Vector2Int.RoundToInt(window.nodeSizes.TryGetValue(node, out var size) ? size : new Vector2(0,0));

        private static Texture2D nodeGroupBody => _nodeGroupBody != null ? _nodeGroupBody : _nodeGroupBody = Resources.Load<Texture2D>("xnode_group");
        private static Texture2D _nodeGroupBody;
        private static GUIStyle guiStyle;

        public override GUIStyle GetBodyStyle() => guiStyle ??= new GUIStyle(base.GetBodyStyle())
        {
            normal =
            {
                background = nodeGroupBody
            }
        };
    }
}
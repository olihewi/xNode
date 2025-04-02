using System;
using System.Collections.Generic;
using UnityEngine;
using XNode.Flow.Nodes;
using XNode.Variables;

namespace XNode.Flow
{
    [CreateAssetMenu(menuName = "FSS/Flow Graph", order = -100)]
    public class FlowGraph : NodeGraph, INodeVariableProvider
    {
        public EntryNode EntryNode;
        public ReturnNode ReturnNode;
        public Dictionary<string, object> NodeVariables { get; set; }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var node in nodes)
            {
                switch (node)
                {
                    case EntryNode entryNode:
                        EntryNode = entryNode;
                        break;
                    case ReturnNode returnNode:
                        ReturnNode = returnNode;
                        break;
                }
            }
        }
    #endif
    }
}
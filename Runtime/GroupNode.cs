using System.Collections.Generic;
using UnityEngine;

namespace XNode
{
    [NodeWidth(208), NodeHeight(208)]
    public class GroupNode : Node
    {
        public Color color = new Color(1.0F,1.0F,1.0F,1.0F);
        public List<Node> children = new List<Node>();
    }
}
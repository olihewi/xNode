using System;

namespace XNode
{
    [Serializable]
    public class NodeObjectWrapper
    {
        public string name;
        public object Value { get; set; }
    }
}
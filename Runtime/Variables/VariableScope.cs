using System;

namespace XNode.Variables
{
    public enum VariableScope
    {
        Global = 1 << 0,
        FlowGraphPlayer = 1 << 1,
        Graph = 1 << 3,
        FlowContext = 1 << 4,
    }

    [Flags]
    public enum VariableScopeFlags
    {
        None = 0,
        Global = 1 << 0,
        FlowGraphPlayer = 1 << 1,
        ParentGraphs = 1 << 2,
        Graph = 1 << 3,
        FlowContext = 1 << 4,
        All = ~0,
    }
}
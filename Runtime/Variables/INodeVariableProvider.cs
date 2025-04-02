using System.Collections.Generic;

namespace XNode.Variables
{
    public interface INodeVariableProvider
    {
        public Dictionary<string, object> NodeVariables { get; set; }
    }

    public static class NodeVariableProviderExtensions
    {
        public static bool TryGetVariable(this INodeVariableProvider variableProvider, string key, out object value)
        {
            value = default;
            return variableProvider.NodeVariables != null && variableProvider.NodeVariables.TryGetValue(key, out value);
        }

        public static bool TryGetVariable<TValue>(this INodeVariableProvider variableProvider, string key, out TValue value)
        {
            value = default;
            if (!TryGetVariable(variableProvider, key, out var objValue)) return false;
            if (objValue is not TValue tVal) return false;
            value = tVal;
            return true;
        }

        public static void SetVariable(this INodeVariableProvider variableProvider, string key, object value)
        {
            variableProvider.NodeVariables ??= new Dictionary<string, object>(1);
            if (!variableProvider.NodeVariables.ContainsKey(key))
                variableProvider.NodeVariables.Add(key, value);
            else variableProvider.NodeVariables[key] = value;
        }

        public static void ClearVariable(this INodeVariableProvider variableProvider, string key)
        {
            if (variableProvider.NodeVariables != null && variableProvider.NodeVariables.ContainsKey(key))
            {
                variableProvider.NodeVariables.Remove(key);
                if (variableProvider.NodeVariables.Count == 0) variableProvider.NodeVariables = null;
            }
        }

        public static void ClearAllVariables(this INodeVariableProvider variableProvider)
        {
            variableProvider.NodeVariables = null;
        }
    }
}
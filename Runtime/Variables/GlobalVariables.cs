using System.Collections.Generic;

namespace XNode.Variables
{
    public static class GlobalVariables
    {
        public static Dictionary<string, object> NodeVariables { get; set; }

        public static bool TryGetVariable(string key, out object value)
        {
            value = default;
            return NodeVariables != null && NodeVariables.TryGetValue(key, out value);
        }

        public static bool TryGetVariable<TValue>(string key, out TValue value)
        {
            value = default;
            if (!TryGetVariable(key, out var objValue)) return false;
            if (objValue is not TValue tVal) return false;
            value = tVal;
            return true;
        }

        public static void SetVariable(string key, object value)
        {
            NodeVariables ??= new Dictionary<string, object>(1);
            if (!NodeVariables.ContainsKey(key))
                NodeVariables.Add(key, value);
            else NodeVariables[key] = value;
        }

        public static void ClearVariable(string key)
        {
            if (NodeVariables != null && NodeVariables.ContainsKey(key))
            {
                NodeVariables.Remove(key);
                if (NodeVariables.Count == 0) NodeVariables = null;
            }
        }

        public static void ClearAllVariables() => NodeVariables = null;
    }
}
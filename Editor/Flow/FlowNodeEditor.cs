
using XNode.Flow;

namespace XNodeEditor.Flow
{
    [CustomNodeEditor(typeof(FlowNode))]
    public class FlowNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            NodeEditorGUILayout.PortPair(target.GetPort("previousNode"), target.GetPort("nextNode"));
            base.OnBodyGUI();
        }
    }
}
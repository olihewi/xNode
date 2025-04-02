using XNode.Flow;

namespace XNode.Nodes
{
    [CreateNodeMenu("Constants/String"), NodeTypeTint(typeof(string))]
    public class StringConstantNode : ConstantNode<string> {}
}
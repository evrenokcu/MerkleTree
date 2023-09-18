namespace MerkleTree;

public class LeafNode : Node
{
    public LeafNode(string value, Node parentNode) : base(value, parentNode, NullNode, NullNode, 0)
    {
    }
}
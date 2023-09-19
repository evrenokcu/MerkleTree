namespace MerkleTree;

internal sealed class LeafNode : Node
{
    private LeafNode(uint id, Node parentNode) : base(id, parentNode, None, None, 0)
    {
    }

    internal static LeafNode Create(uint id, Node parentNode) => new(id, parentNode);
}
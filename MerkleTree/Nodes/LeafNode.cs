namespace MerkleTree.Nodes;

internal sealed class LeafNode : Node
{
    private LeafNode(string value, Node parentNode) : base( value, parentNode, None, None, 0)
    {
    }

    internal static LeafNode Create( string value, Node parentNode) => new( value, parentNode);

    internal override void RecalculateHash(Func<string, string> hashFunction)
    {
        Parent!.RecalculateHash(hashFunction);
    }
}
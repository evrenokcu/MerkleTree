namespace MerkleTree;

internal sealed class LeafNode : Node
{
    private LeafNode(uint id, string value, Node parentNode) : base(id,value, parentNode, None, None, 0)
    {
    }

    internal static LeafNode Create(uint id,string value, Node parentNode) => new(id,value, parentNode);

    internal override void RecalculateHash(Func<string, string> hashFunction)
    {
        Parent!.RecalculateHash(hashFunction);
    }
}
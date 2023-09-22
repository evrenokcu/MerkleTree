namespace MerkleTree.Nodes;

internal class NullNode : Node
{
    private NullNode() : base(0, string.Empty, null, null, null, 0)
    {
    }

    internal override void RecalculateHash(Func<string, string> hashFunction)
    {
        //do nothing
    }

    internal static NullNode Create() => new();

}
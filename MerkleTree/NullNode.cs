namespace MerkleTree;

internal class NullNode:Node
{
    internal NullNode() : base(0, string.Empty, null, null, null, 0)
    {
    }

    internal override void RecalculateHash(Func<string, string> hashFunction)
    {
        //do nothing
    }
}
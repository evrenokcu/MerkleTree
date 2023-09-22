namespace MerkleTree.Api;

public sealed class NodeInformation
{
    public string HashValue { get; init; }
    public uint TreeLevel { get; init; }

    private NodeInformation(string hashValue, uint treeLevel)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(hashValue));
       
        HashValue = hashValue;
        TreeLevel = treeLevel;
    }

    public static NodeInformation Of(string hashValue, uint treeLevel) =>
        new(hashValue, treeLevel);
}
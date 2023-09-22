namespace MerkleTree.Api;

public sealed class NodeInformation
{
    public uint NodeId { get; init; }
    public string HashValue { get; init; }
    public uint TreeLevel { get; init; }

    private NodeInformation(uint nodeId, string hashValue, uint treeLevel)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(hashValue));

        NodeId = nodeId;
        HashValue = hashValue;
        TreeLevel = treeLevel;
    }

    public static NodeInformation Of(uint nodeId, string hashValue, uint treeLevel) =>
        new(nodeId, hashValue, treeLevel);
}
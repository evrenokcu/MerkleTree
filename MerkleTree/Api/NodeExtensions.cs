using MerkleTree.Nodes;

namespace MerkleTree.Api;

internal static class NodeExtensions
{
    public static NodeInformation Convert(this Node node) => NodeInformation.Of(node.Id, node.Value, node.Level);

}
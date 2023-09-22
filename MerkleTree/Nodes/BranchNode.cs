namespace MerkleTree.Nodes;

internal class BranchNode : Node
{
    public BranchNode(uint id, string value, Node parent) : base(id, value, parent, parent.Right, None, parent.GetSubNodeCount())
    {
    }
    public static BranchNode Create(uint id, string value, Node parentOf) =>new (id, value, parentOf );
}
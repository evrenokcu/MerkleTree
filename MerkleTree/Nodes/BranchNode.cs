namespace MerkleTree.Nodes;

internal class BranchNode : Node
{
    public BranchNode( string value, Node parent) : base(value, parent, parent.Right, None, parent.GetSubNodeCount())
    {
    }
    public static BranchNode Create(string value, Node parentOf) =>new (value, parentOf );
}
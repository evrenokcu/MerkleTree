namespace MerkleTree.Nodes;

internal class RootNode : Node
{
    private RootNode(uint id, string value, Node left, uint level) : base(id, value, None, left, None, level)
    {
        if (left != None)
        {
            left.SetParent(this);
        }

    }
    internal static RootNode CreateFirstRoot => new(1, string.Empty, None, 1);

    internal static RootNode Create(uint id, string value, Node parentOf)=>
        new (id, value, parentOf, parentOf.Level + 1);
    
}
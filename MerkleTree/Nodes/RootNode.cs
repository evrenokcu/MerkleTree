namespace MerkleTree.Nodes;

internal class RootNode : Node
{
    private RootNode( string value, Node left, uint level) : base( value, None, left, None, level)
    {
        if (left != None)
        {
            left.SetParent(this);
        }

    }
    internal static RootNode CreateFirstRoot => new( string.Empty, None, 1);

    internal static RootNode Create(string value, Node parentOf)=>
        new (value, parentOf, parentOf.Level + 1);
    
}
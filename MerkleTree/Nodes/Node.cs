namespace MerkleTree.Nodes;

internal abstract class Node
{
    internal static readonly Node None = NullNode.Create();
    internal string Value { get; private set; }
    internal Node? Left { get; private set; }
    internal Node? Right { get; private set; }
     internal Node? Parent { get; private set; }
    internal uint Level { get; }
    internal uint AllowedSubNodeCount { get; private set; }
    internal bool HasRoomForNonLeaf() => AllowedSubNodeCount > 0;
    internal bool IsRoot() => Parent == None;
    internal bool HasRoom() => HasRoomForNonLeaf() || HasRoomForLeaf();
    internal bool HasRoomForLeaf() => Left == None || Right == None;
    internal uint GetSubNodeCount() => Level - AllowedSubNodeCount;

    internal void SetParent(Node node)
    {
        Parent = node;
    }
    internal Node(string value, Node? parent, Node? left, Node? right, uint level)
    {
        Parent = parent;
        Left = left;
        Right = right;
        Level = level;
        AllowedSubNodeCount = level - 1;
        Value = value;
    }
    
    internal static RootNode CreateRootNode( string value, Node parentOf)
    {
        var node = RootNode.Create(value, parentOf);
        parentOf.Parent = node;
        return node;
    }


    internal LeafNode AssignLeafNode(string value)
    {
        if (!HasRoomForLeaf()) throw new InvalidOperationException("Node is already full, can not accept a leaf.");

        LeafNode newNode = LeafNode.Create( value, this);

        if (Left == None)
        {
            Left = newNode;
        }
        else if (Right == None)
        {
            Right = newNode;
        }
        else throw new InvalidOperationException();

        return newNode;
    }



    internal void AddBranchNode(Node child)
    {
        AllowedSubNodeCount -= 1;
        Right = child;
    }

    internal virtual void RecalculateHash(Func<string, string> hashFunction)
    {
        Value = hashFunction.Invoke(string.Concat(Left!.Value, Right!.Value));
        Parent!.RecalculateHash(hashFunction);
    }
}
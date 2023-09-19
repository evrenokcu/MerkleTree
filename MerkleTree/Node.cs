namespace MerkleTree;

internal class Node
{
    internal static readonly Node None = CreateNullNode();
    internal string Value { get; private set; }
    internal uint Id { get; private set; }
    internal Node? Left { get; private set; }
    internal Node? Right { get; private set; }
    internal Node? Parent { get; private set; }
    internal uint Level { get; }
    internal uint AllowedSubNodeCount { get; private set; }
    internal bool HasRoomForNonLeaf() => AllowedSubNodeCount > 0;
    internal bool IsRoot() => Parent == None;
    internal bool HasRoom() => HasRoomForNonLeaf() || HasRoomForLeaf();
    internal bool HasRoomForLeaf() => Left == None || Right == None;
    private uint GetSubNodeCount() => (Level - AllowedSubNodeCount);

    protected Node(uint id, string value, Node? parent, Node? left, Node? right, uint level)
    {
        Parent = parent;
        Id = id;
        Left = left;
        Right = right;
        Level = level;
        AllowedSubNodeCount = level - 1;
        Value = value;
    }


    private static NullNode CreateNullNode() => new();

    internal static Node CreateChildNode(uint id, string value, Node parentNode) =>
        new(id, value, parentNode, parentNode.Right, Node.None, parentNode.GetSubNodeCount());


    internal static Node CreateRootNode(uint id, string value, Node parentOf)
    {
        var node = new Node(id, value, Node.None, parentOf, None, parentOf.Level + 1);
        parentOf.Parent = node;
        return node;
    }


    internal LeafNode AssignLeafNode(uint id, string value)
    {
        if (!HasRoomForLeaf()) throw new InvalidOperationException("Node is already full, can not accept a leaf.");

        LeafNode newNode = LeafNode.Create(id, value, this);

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

    internal static Node CreateFirstRoot(uint id, string value)
    {
        return new Node(id, value, None, None, None, 1);
    }

    internal void AddChildNode(Node child)
    {
        AllowedSubNodeCount -= 1;
        Right = child;
    }

    internal virtual void RecalculateHash(Func<string, string> hashFunction)
    {
        this.Value = hashFunction.Invoke(string.Concat(Left!.Value, Right!.Value));
        Parent!.RecalculateHash(hashFunction);
    }
}
using System.ComponentModel;

namespace MerkleTree;

internal class Node
{
    internal static readonly Node None = CreateNullNode();
    internal uint Id { get; private set; }
    internal Node? Left { get; private set; }
    internal Node? Right { get; private set; }
    internal Node? Parent { get; private set; }
    internal int Level { get; }
    internal int AllowedSubNodeCount { get; private set; }
    internal bool HasRoomForNonLeaf() => AllowedSubNodeCount > 0;
    internal bool IsRoot() => Parent == None;
    internal bool HasRoom() => HasRoomForNonLeaf() || HasRoomForLeaf();
    internal bool HasRoomForLeaf() => Left == None || Right == None;
    private int GetSubNodeCount() => Level - AllowedSubNodeCount;

    protected Node(uint id, Node? parent, Node? left, Node? right, int level)
    {
        if (level < 0) throw new InvalidEnumArgumentException(nameof(level));
        Parent = parent;
        Id = id;
        Left = left;
        Right = right;
        Level = level;
        AllowedSubNodeCount = level - 1;
    }


    private static Node CreateNullNode() => new(0, null, null, null, 0);

    internal static Node CreateChildNode(uint id, Node parentNode) =>
        new(id, parentNode, parentNode.Right, Node.None, parentNode.GetSubNodeCount());


    internal static Node CreateRootNode(uint id, Node parentOf)
    {
        var node = new Node(id, Node.None, parentOf, None, parentOf.Level + 1);
        parentOf.Parent = node;
        return node;
    }


    internal LeafNode AssignLeafNode(uint id)
    {
        if (!HasRoomForLeaf()) throw new InvalidOperationException($"Node is already full, can not accept a leaf.");

        LeafNode newNode = LeafNode.Create(id, this);

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

    internal static Node CreateFirstRoot(uint id)
    {
        return new Node(id, None, None, None, 1);
    }

    internal void AddChildNode(Node child)
    {
        AllowedSubNodeCount += -1;
        Right = child;
    }
}
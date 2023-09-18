using System.ComponentModel;

namespace MerkleTree;

public class Node
{
    public static readonly Node NullNode = CreateNullNode();
    public string Value { get; private set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }
    public Node? Parent { get; private set; }
    public int Level { get; }
    public int AllowedSubParents { get; private set; }

    protected Node(string value, Node? parent, Node? left, Node? right, int level)
    {
        if (level < 0) throw new InvalidEnumArgumentException(nameof(level));
        Parent = parent;
        Value = value;
        Left = left;
        Right = right;
        Level = level;
        AllowedSubParents = level-1;
    }

    private static Node CreateNullNode() => new(string.Empty, null, null, null, 0);

    internal static Node CreateChildNode(string value, Node parentNode) =>
        new(value, parentNode, parentNode.Right, Node.NullNode, parentNode.Level- parentNode.AllowedSubParents);

    internal static Node CreateRoot(string value, Node parentOf)
    {
        var node = new Node(value, Node.NullNode, parentOf, NullNode, parentOf.Level + 1);
        parentOf.Parent = node;
        return node;
    }

    internal bool HasRoom() => Left == NullNode || Right == NullNode;

    internal LeafNode Accept(string value)
    {
        if (!HasRoom()) throw new InvalidOperationException($"Node is already full, can not accept a leaf.");

        LeafNode newNode = new LeafNode(value, this);

        if (Left == NullNode)
        {
            //todo: use static factory on node when creating new leaf node
            Left = newNode;
        }
        else if (Right == NullNode)
        {
            Right = new LeafNode(value, this);
        }
        else throw new InvalidOperationException();

        return newNode;
    }

    internal bool CanHaveSubParent() => AllowedSubParents > 0;

    public static Node CreateFirstRoot(string value)
    {
        return new Node(value, NullNode, NullNode, NullNode, 1);
    }

    public void AddChild(Node child)
    {
        AllowedSubParents += -1;
        Right = child;
    }
}
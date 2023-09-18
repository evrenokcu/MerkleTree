using System.ComponentModel;
using System.Xml;

namespace MerkleTree;

internal class Queue
{
    private Stack<Node> _parentNodeQueue = new();

    public void Push(Node node) => _parentNodeQueue.Push(node);

    //todo, check what happens if stack is empty
    public Node Pop()
    {
        if(_parentNodeQueue.Count==0) return Node.NullNode;
        return _parentNodeQueue.Pop();
    }
}

public class Tree
{
    public int LeafCount = 0;
    public Node RootNode { get; private set; } = Node.NullNode;
    public Node CurrentParentNode { get; private set; } = Node.NullNode;
    private Queue _parentNodesQueue = new Queue();
    public int NodeNumber = 0;


    public Tree()
    {
    }

    public void AddNode(string value)
    {
        if (CurrentParentNode == Node.NullNode)
        {
            var newRoot = Node.CreateFirstRoot((++NodeNumber).ToString());
            RootNode = newRoot;
            CurrentParentNode = newRoot;
        }

        AddNode(value, _parentNodesQueue,CurrentParentNode);
        LeafCount++;
    }

    private void AddNode(string value, Queue parentNodesStack, Node currentParentNode)
    {


        if (currentParentNode.HasRoom())
        {
            currentParentNode.Accept(value);
            CurrentParentNode = currentParentNode;
            return;
            //return currentParentNode;
        }


        if (currentParentNode.CanHaveSubParent())
        {
            var newChild=Node.CreateChildNode((++NodeNumber).ToString(), currentParentNode);
            currentParentNode.AddChild(newChild);
            parentNodesStack.Push(currentParentNode);
            AddNode(value,parentNodesStack,newChild);
            //CurrentParentNode = newChild;
            return;
        }

        var parentInQueue = parentNodesStack.Pop();
        if (parentInQueue != Node.NullNode)
        {
             AddNode(value, parentNodesStack, parentInQueue);
             //CurrentParentNode = parentInQueue;
             return;
        }

        Node newRoot =  Node.CreateRoot((++NodeNumber).ToString(), currentParentNode);
        AddNode(value, parentNodesStack, newRoot);
        RootNode = newRoot;
        //CurrentParentNode = newRoot;

    }


    public void AddLeafNode(string nodeValue, Func<string, string> hashFunction)
    {
        var hashedValue = hashFunction.Invoke(nodeValue);
        LeafCount++;
        //var newNode=Node.CreateLeafNode
    }
}

public class Node
{
    public static readonly Node NullNode = CreateNullNode();
    public string Value { get; private set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }
    public Node? Parent { get; private set; }
    public int Level { get; private set; } = 0;
    public int AllowedSubParents { get; private set; } = 0;

    protected Node(string value, Node? parent, Node? left, Node? right, int level)
    {
        if (level < 0) throw new InvalidEnumArgumentException(nameof(level));
        Parent = parent;
        Value = value;
        Left = left;
        Right = right;
        Level = level;
        AllowedSubParents = level;
    }

    private static Node CreateNullNode() => new(string.Empty, null, null, null, 0);

    internal static Node CreateChildNode(string value, Node parentNode) =>
        new(value, parentNode, parentNode.Right, Node.NullNode, parentNode.Level- parentNode.AllowedSubParents);

    internal static Node CreateRoot(string value, Node parentOf)
    {
        var node = new Node(value, Node.NullNode, parentOf, NullNode, parentOf.Level + 1);
        return node;
    }

    internal bool HasRoom() => Left == NullNode || Right == NullNode;

    internal void Accept(string value)
    {
        if (Left == NullNode)
        {
            Left = new LeafNode(value, this);

            return;
        }

        if (Right == NullNode)
        {
            Right = new LeafNode(value, this);
            return;
        }
    }

    internal bool CanHaveSubParent() => AllowedSubParents > 0;

    public static Node CreateFirstRoot(string value)
    {
        return new Node(value, NullNode, NullNode, NullNode, 0);
    }

    public void AddChild(Node child)
    {
        AllowedSubParents += -1;
        Right = child;
    }
}

public class LeafNode : Node
{
    public LeafNode(string value, Node parentNode) : base(value, parentNode, NullNode, NullNode, 0)
    {
    }
}
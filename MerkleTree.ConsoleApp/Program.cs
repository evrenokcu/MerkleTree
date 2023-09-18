using MerkleTree.Visitors.Implementations;

namespace MerkleTree.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var tree = new Tree();
        for (int i = 1; i < 33; i++)
        {
            tree.AddNode(i.ToString());

        }
        var leafCount = tree.LeafCount;
        var rootNode = tree.RootNode;
        var currentParentNode = tree.CurrentParentNode;

        Console.WriteLine($"Leaf count:{leafCount}");

        VisitToParent(tree.GetLeaf(1));
        VisitTree(rootNode);

        Console.ReadLine();
    }

    private static void VisitTree(Node rootNode)
    {
        var visitor = new LevelOrderVisitor(rootNode, (node) => Console.Write($" {node.Value} "));
        visitor.Visit();
    }
    private static void VisitToParent(LeafNode leaf)
    {
        var visitor = new LeafToParentVisitor(leaf, (node) => Console.Write($" {node.Value} "));
        visitor.Visit();
    }

    public static void WriteLevel(Node node,int level)
    {
        if (node == Node.NullNode) return;
        if(node.Level==level)
            Console.Write($" {node.Value} ");
        WriteLevel(node.Left,level);
        WriteLevel(node.Right,level);

    }

    public static void WriteNode(Node node, string prefix)
    {
        Stack<Node> stack = new Stack<Node>();
            
        stack.Push(node);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            if (node != Node.NullNode)
            {
                Console.WriteLine($"{prefix} Level:{node.Level}, Value:{node.Value}");
            }
        }
        if (node == Node.NullNode) return;
        Console.WriteLine($"{prefix} Level:{node.Level}, Value:{node.Value}");
        WriteNode(node.Left,"left:");
        WriteNode(node.Right,"right:");
    }
}
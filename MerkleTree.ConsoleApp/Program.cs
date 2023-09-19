using MerkleTree.Visitors.Implementations;

namespace MerkleTree.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var tree = new Tree(it => it);
        for (uint i = 1; i <=32 ; i++)
        {
            tree.AddNode(i.ToString());

        }
        var leafCount = tree.LeafCount;
        var rootNode = tree.RootNode;
       

        Console.WriteLine($"Leaf count:{leafCount}");

        VisitToParent(tree.GetLeaf(1));
        VisitTree(rootNode);

        Console.ReadLine();
    }

    private static void VisitTree(Node rootNode)
    {
        var level = rootNode.Level;
        Console.WriteLine();
        var visitor = new LevelOrderVisitor(rootNode, (node) =>
        {
            if (node.Level != level)
            {
                level=node.Level;
                Console.WriteLine();
            }
            Console.Write($" {node.Id} Value:{node.Value} ");
        });
        visitor.Visit();
    }
    private static void VisitToParent(LeafNode leaf)
    {
        var visitor = new LeafToParentVisitor(leaf, (node) => Console.Write($" Level:{node.Level},Id:{node.Id} "));
        visitor.Visit();
    }

    public static void WriteLevel(Node node, uint level)
    {
        if (node == Node.None) return;
        if(node.Level==level)
            Console.Write($" {node.Id} ");
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
            if (node != Node.None)
            {
                Console.WriteLine($"{prefix} Level:{node.Level}, Id:{node.Id}");
            }
        }
        if (node == Node.None) return;
        Console.WriteLine($"{prefix} Level:{node.Level}, Id:{node.Id}");
        WriteNode(node.Left,"left:");
        WriteNode(node.Right,"right:");
    }
}
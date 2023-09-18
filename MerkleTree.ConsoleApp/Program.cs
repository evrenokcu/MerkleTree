using System.Xml;

namespace MerkleTree.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            var tree = new Tree();
            for (int i = 1; i < 33; i++)
            {
                tree.AddNode(i.ToString());

            }
            var leafCount = tree.LeafCount;
            var rootNode = tree.RootNode;
            var currentParentNode = tree.CurrentParentNode;

            VisitTree(rootNode);

            WriteNode(rootNode, "Root");
            WriteLevel(rootNode, 4);
            Console.WriteLine();
            WriteLevel(rootNode, 3);
            Console.WriteLine();
            WriteLevel(rootNode, 2);
            Console.WriteLine();
            WriteLevel(rootNode, 1);
            Console.WriteLine();
            WriteLevel(rootNode, 0);
            Console.WriteLine();



            Console.ReadLine();
        }

        private static void VisitTree(Node rootNode)
        {
            Queue<Node> queue= new Queue<Node>();
            queue.Enqueue(rootNode);

            var nodeCount = 1;

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node != Node.NullNode)
                {
                    Console.Write($" {node.Value} ");
                    queue.Enqueue(node.Left);
                    queue.Enqueue(node.Right);
                }
            }
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
}
namespace MerkleTree.ClientConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tree = new Tree(it => it);
            for (uint i = 1; i <= 32; i++)
            {
                tree.AddNode(Convert.ToString(i));
                
            }
        }
    }
}
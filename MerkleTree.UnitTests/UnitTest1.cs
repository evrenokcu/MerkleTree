using FluentAssertions;

namespace MerkleTree.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateNullNode()
        {
            var node = Node.NullNode;
            node.Value.Should().Be(string.Empty);
            node.Left.Should().Be(null);
            node.Right.Should().Be(null);
            node.Parent.Should().Be(null);
        }

        [Fact]
        public void EmptyTreeShouldHaveNoNodes()
        {
            var tree = new Tree();
            tree.LeafCount.Should().Be(0);
            tree.RootNode.Should().Be(Node.NullNode);
        }

        [Fact]
        public void Fill()
        {
            var tree = new Tree();
            for (int i = 1; i < 33; i++)
            {
                tree.AddNode(i.ToString());
                
            }
            var leafCount=tree.LeafCount;
            var rootNode=tree.RootNode;
            var currentParentNode = tree.CurrentParentNode;

            Console.WriteLine(tree.RootNode.Value);
        }
    }
}
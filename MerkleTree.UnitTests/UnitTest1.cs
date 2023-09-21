using FluentAssertions;

namespace MerkleTree.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void CanCreateNullNode()
        {
            var node = Node.None;
            node.Id.Should().Be(0);
            node.Left.Should().Be(null);
            node.Right.Should().Be(null);
            node.Parent.Should().Be(null);
        }

        [Fact]
        public void EmptyTreeShouldHaveNoNodes()
        {
            var tree = new Tree(it=>it);
            tree.LeafCount.Should().Be(0);
            tree.RootNode.Should().Be(Node.None);
        }

        [Fact]
        public void Fill()
        {
            var tree = new Tree(it=>it);
            for (uint i = 1; i <= 32; i++)
            {
                tree.AddNode(i.ToString());
                
            }
            var leafCount=tree.LeafCount;
            var rootNode=tree.RootNode;
            var currentParentNode = tree.CurrentNode;

            Console.WriteLine(tree.RootNode.Id);
        }
    }
}
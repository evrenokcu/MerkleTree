using MerkleTree.Nodes;

namespace MerkleTree.Visitors.Implementations;

internal sealed class LeafToParentVisitor : ITreeVisitor
{
    private readonly Action<Node> _action;
    private readonly LeafNode _leafNode;

    public void Visit()
    {
        Node node = _leafNode;
        do
        {
            _action.Invoke(node);
            node = node.Parent!;
        } while (node != Node.None);
    }

    public LeafToParentVisitor(LeafNode leafNode, Action<Node> action)
    {
        _leafNode = leafNode;
        _action = action;
    }
}
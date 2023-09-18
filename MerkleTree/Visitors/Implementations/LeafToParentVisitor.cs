namespace MerkleTree.Visitors.Implementations;

public class LeafToParentVisitor : ITreeVisitor
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
        } while (node != Node.NullNode);
    }

    public LeafToParentVisitor(LeafNode leafNode, Action<Node> action)
    {
        _leafNode = leafNode;
        _action = action;
    }
}
namespace MerkleTree;

internal sealed class NodeStack
{
    private readonly Stack<Node> _parentNodeQueue = new();

    public void Push(Node node) => _parentNodeQueue.Push(node);

    public Node Pop()
    {
        if(_parentNodeQueue.Count==0) return Node.None;
        return _parentNodeQueue.Pop();
    }
}
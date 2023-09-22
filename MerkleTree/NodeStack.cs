using MerkleTree.Nodes;

namespace MerkleTree;

internal sealed class NodeStack
{
    private readonly Stack<Node> _parentNodeQueue = new();

    public void Push(Node node) => _parentNodeQueue.Push(node);

    public Node Pop()=>
         _parentNodeQueue.TryPop(out var node)?node:Node.None;
}
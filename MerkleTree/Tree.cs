namespace MerkleTree;

public sealed class Tree
{
    private readonly IList<LeafNode> _leaves = new List<LeafNode>();
    private readonly TreeBuilder _builder;

    internal Node RootNode { get; private set; }

    internal int LeafCount { get; private set; }
    private readonly Func<string, string> _hashFunction;

    public Tree(Func<string, string> hashFunction)
    {
        _builder = new TreeBuilder(this);
        _hashFunction = hashFunction;
    }

    public void AddNode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

        _builder.DoAddNode(_hashFunction.Invoke(value));
    }

    public NodeInformation GetLeafInformation(int leafNumber) => GetLeaf(leafNumber).Convert();

    public NodeInformation ChangeLeafHash(int leafNumber)
    {
        var leaf = GetLeaf(leafNumber);
        leaf.RecalculateHash(_hashFunction);
        return leaf.Convert();
    }

    public NodeInformation GetRootInformation() => RootNode.Convert();

    internal void AddLeaf(LeafNode node)
    {
        _leaves.Add(node);
        LeafCount++;
        node.RecalculateHash(_hashFunction);
    }

    internal LeafNode GetLeaf(int leafNumber)
    {
        if (leafNumber == 0 || leafNumber > LeafCount) throw new ArgumentOutOfRangeException(nameof(leafNumber));
        return _leaves[leafNumber - 1];
    }

    internal void SetRootNode(Node node)
    {
        RootNode = node;
    }
}
using MerkleTree.Api;
using MerkleTree.Nodes;

namespace MerkleTree;

public sealed class Tree:ITree
{
    private readonly IList<LeafNode> _leaves = new List<LeafNode>();
    private readonly TreeBuilder _builder;

    internal RootNode RootNode { get; private set; }

    internal int LeafCount { get; private set; }
    private readonly Func<string, string> _hashFunction;

    public Tree(Func<string, string> hashFunction)
    {
        _builder = new TreeBuilder();
        _hashFunction = hashFunction;
        RootNode=_builder.Root;
    }

    public void AddNode(string value)
    { 
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

       var (leaf, root) = _builder.AddNode(_hashFunction.Invoke(value));
       
       _leaves.Add(leaf);
       LeafCount++;
       leaf.RecalculateHash(_hashFunction);
       RootNode = root;
    }

    public NodeInformation GetLeafInformation(int leafNumber) => GetLeaf(leafNumber).Convert();

    public NodeInformation GetRootInformation() => RootNode.Convert();

    internal LeafNode GetLeaf(int leafNumber)
    {
        if (leafNumber == 0 || leafNumber > LeafCount) throw new ArgumentOutOfRangeException(nameof(leafNumber));
        return _leaves[leafNumber - 1];
    }
    
}
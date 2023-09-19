namespace MerkleTree;

public sealed class Tree
{
    private readonly Func<string, string> _hashFunction;
    private readonly NodeStack _parentNodesNodeStack = new();
    private uint _nodeNumber = 1;
    private uint _leafNodeNumber = 0;
    private readonly IList<LeafNode> _leaves = new List<LeafNode>();

    internal Node RootNode { get; private set; }
    internal Node CurrentParentNode { get; private set; }
    internal int LeafCount { get; private set; }

    public Tree(Func<string, string> hashFunction)
    {
        _hashFunction = hashFunction;
        RootNode = Node.CreateFirstRoot(1, string.Empty);
        CurrentParentNode = RootNode;
        _parentNodesNodeStack.Push(RootNode);
    }

    public (NodeInformation rootNode,NodeInformation leafNode) AddNode(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
        DoAddNode(
            value,
            _parentNodesNodeStack,
            CurrentParentNode,
            () => ++_nodeNumber,
            ++_leafNodeNumber,
            _hashFunction,
            AddLeaf,
            root => RootNode = root,
            parentNode => CurrentParentNode = parentNode
        );
        

        return (RootNode.Convert(), GetLeaf((int)_leafNodeNumber).Convert());
    }

    public NodeInformation GetLeafInformation(int leafNumber) => GetLeaf(leafNumber).Convert();
    public NodeInformation ChangeLeafHash(int leafNumber)
    {
        var leaf = GetLeaf(leafNumber);
        leaf.RecalculateHash(_hashFunction);
        return leaf.Convert();
    }

    public NodeInformation GetRootInformation() => RootNode.Convert();

    private void AddLeaf(LeafNode node)
    {
        _leaves.Add(node);
        LeafCount++;
    }
    internal LeafNode GetLeaf(int leafNumber)
    {
        if (leafNumber == 0 || leafNumber > LeafCount) throw new ArgumentOutOfRangeException(nameof(leafNumber));
        return _leaves[leafNumber - 1];
    }

    private static void DoAddNode(string value,
        NodeStack parentNodesStack,
        Node currentParentNode,
        Func<uint> nodeNumberProvider,
        uint leafNumber,
        Func<string, string> hashFunction,
        Action<LeafNode>? onNewLeaf = null,
        Action<Node>? onNewRoot = null,
        Action<Node>? onChangeCurrentNode = null)
    {
        var currentNode = currentParentNode;

#if DEBUG
        var iterationCount = 0;
#endif
        while (!currentNode.HasRoomForLeaf())
        {
#if DEBUG
            iterationCount++;
            if (iterationCount > 1)
            {
                throw new Exception("iteration has run more than once!");
            }
#endif
            if (currentNode.HasRoomForNonLeaf())
            {
                var newChild = Node.CreateChildNode(nodeNumberProvider.Invoke(), value, currentNode);
                currentNode.AddChildNode(newChild);
                //if current node can have additional sub parents, push it to stack to return back
                if (currentNode.HasRoomForNonLeaf() || currentNode.IsRoot())
                {
                    parentNodesStack.Push(currentNode);
                }

                currentNode = newChild;
            }
            else if (currentNode.IsRoot() && !currentNode.HasRoom())
            {
                //no room, create another root
                Node newRoot = Node.CreateRootNode(nodeNumberProvider.Invoke(), value, currentNode);
                onNewRoot?.Invoke(newRoot);
                currentNode = newRoot;
            }
            else
            {
                currentNode = parentNodesStack.Pop();
            }
        }

        if (!currentNode.HasRoomForLeaf())
            throw new Exception();


        var newLeaf = currentNode.AssignLeafNode(leafNumber, hashFunction.Invoke(value));
        newLeaf.RecalculateHash(hashFunction);
        onNewLeaf?.Invoke(newLeaf);

        if (!currentNode.HasRoom())
        {
            //Can not accept either leaf or non-leaf
            //we are done with this node. 
            //get next node from queue
            currentNode = parentNodesStack.Pop();
        }

        onChangeCurrentNode?.Invoke(currentNode);
        //return currentNode;
    }


    //public void AddLeafNode(string nodeValue, Func<string, string> hashFunction)
    //{
    //    var hashedValue = hashFunction.Invoke(nodeValue);
    //    LeafCount++;
    //}
}
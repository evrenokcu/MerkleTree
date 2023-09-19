namespace MerkleTree;

public sealed class Tree
{
    private readonly NodeStack _parentNodesNodeStack = new();
    private uint _nodeNumber = 1;
    private readonly IList<LeafNode> _leaves = new List<LeafNode>();

    internal Node RootNode { get; private set; }
    internal Node CurrentParentNode { get; private set; }
    internal int LeafCount { get; private set; }

    public Tree()
    {
        RootNode = Node.CreateFirstRoot(1);
        CurrentParentNode = RootNode;
        _parentNodesNodeStack.Push(RootNode);
    }
    public void AddNode(uint id)
    {
        CurrentParentNode = AddNode(
            id,
            _parentNodesNodeStack,
            CurrentParentNode,
            () => ++_nodeNumber,
            _leaves.Add,
            root => RootNode = root,
            parentNode => CurrentParentNode = parentNode);
        LeafCount++;
    }

    internal LeafNode GetLeaf(int leafNumber)
    {
        if (leafNumber == 0 || leafNumber > LeafCount) throw new ArgumentOutOfRangeException(nameof(leafNumber));
        return _leaves[leafNumber - 1];
    }

    private static Node AddNode(
        uint id,
        NodeStack parentNodesStack,
        Node currentParentNode,
        Func<uint> nodeNumberProvider,
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
                var newChild = Node.CreateChildNode(nodeNumberProvider.Invoke(), currentNode);
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
                Node newRoot = Node.CreateRootNode(nodeNumberProvider.Invoke(), currentNode);
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


        var newLeaf = currentNode.AssignLeafNode(id);
        onNewLeaf?.Invoke(newLeaf);

        if (!currentNode.HasRoom())
        {
            //Can not accept either leaf or non-leaf
            //we are done with this node. 
            //get next node from queue
            currentNode = parentNodesStack.Pop();
        }

        onChangeCurrentNode?.Invoke(currentNode);
        return currentNode;
    }


    //public void AddLeafNode(string nodeValue, Func<string, string> hashFunction)
    //{
    //    var hashedValue = hashFunction.Invoke(nodeValue);
    //    LeafCount++;
    //}
}
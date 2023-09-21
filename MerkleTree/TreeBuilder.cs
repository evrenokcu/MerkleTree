namespace MerkleTree;

internal class TreeBuilder
{
    private readonly Tree _tree;
    
    private uint _nodeNumber = 1;
    private uint _leafNodeNumber;

    internal Node CurrentNode { get; private set; }

    private readonly NodeStack _parentNodesStack = new();

    public TreeBuilder(Tree tree)
    {
        _tree = tree;
        var node = CreateFirstRoot(1, string.Empty);
        CurrentNode= node;
        tree.SetRootNode(node);
    }

    internal Node CreateFirstRoot(uint id, string value)
    {
        var node = new Node(id, value, Node.None, Node.None, Node.None, 1);
        _parentNodesStack.Push(node);
        return node;
    }

    internal void DoAddNode(string value)
    {
        if (!CurrentNode.HasRoomForLeaf())
        {
            CurrentNode = FindOrCreateParentNode(value);
        }


        var newLeaf = CurrentNode.AssignLeafNode(++_leafNodeNumber, value);
        
        _tree.AddLeaf(newLeaf);

        if (!CurrentNode.HasRoom())
        {
            //Can not accept either leaf or non-leaf
            //we are done with this node. 
            //get next node from queue
            CurrentNode = _parentNodesStack.Pop();
        }
    }

    private Node FindOrCreateParentNode(string value)
    {
        Node newNode;
        if (CurrentNode.HasRoomForNonLeaf())
        {
            newNode = AddParentNode(value, CurrentNode, _parentNodesStack, ++_nodeNumber);
        }

        else if (CurrentNode.IsRoot() && !CurrentNode.HasRoom())
        {
            //no room, create another root
            newNode = CreateNewRootNode(value, CurrentNode, ++_nodeNumber);
            _tree.SetRootNode(newNode);
        }
        else newNode = _parentNodesStack.Pop();

        if (!newNode.HasRoomForLeaf())
            throw new Exception();
        return newNode;
    }

    private static Node CreateNewRootNode(string value, Node currentNode, uint nodeNumber)
    {
        Node newRoot = Node.CreateRootNode(nodeNumber, value, currentNode);
        return newRoot;
    }

    private static Node AddParentNode(string value, Node currentNode, NodeStack stack, uint nodeNumber)
    {
        var newChild = Node.CreateChildNode(nodeNumber, value, currentNode);
        currentNode.AddChildNode(newChild);
        //if current node can have additional sub parents, push it to stack to return back
        if (currentNode.HasRoomForNonLeaf() || currentNode.IsRoot())
        {
            stack.Push(currentNode);
        }

        return newChild;
    }
}
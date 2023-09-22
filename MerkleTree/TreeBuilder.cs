namespace MerkleTree;

internal class TreeBuilder
{
    private uint _nodeNumber = 1;
    private uint _leafNodeNumber;
    private Node _root;

    private readonly NodeStack _parentNodesStack = new();

    public TreeBuilder()
    {
        var node = CreateFirstRoot(1, string.Empty);
        _root = node;
        _parentNodesStack.Push(node);
    }

    internal static Node CreateFirstRoot(uint id, string value)
    {
        var node = new Node(id, value, Node.None, Node.None, Node.None, 1);
        return node;
    }

    internal (LeafNode leaf,Node root) AddNode(string value)
    {
        var currentNode = _parentNodesStack.Pop();

        if (currentNode == Node.None) throw new Exception();

        if (!currentNode.HasRoomForLeaf())
        {
            var newNode = CreateParentNode(value, currentNode, ++_nodeNumber);
            if (newNode.IsRoot()) _root=newNode;

            if (currentNode.HasRoomForNonLeaf() || currentNode.IsRoot())
                _parentNodesStack.Push(currentNode);
            currentNode = newNode;
        }

        var newLeaf = currentNode.AssignLeafNode(++_leafNodeNumber, value);

        

        if (currentNode.HasRoom() || currentNode.IsRoot())
        {
            //Can not accept either leaf or non-leaf
            //we are done with this node. 
            //get next node from queue
            _parentNodesStack.Push(currentNode);
        }

        return (newLeaf,_root);
    }

    private static Node CreateParentNode(string value, Node node, uint nodeNumber)
    {
        if (node.HasRoomForNonLeaf())
        {
            return AddParentNode(value, node, nodeNumber);
        }

        //no room, create another root
        return CreateNewRootNode(value, node, nodeNumber);

    }

    private static Node CreateNewRootNode(string value, Node currentNode, uint nodeNumber)
    {
        return Node.CreateRootNode(nodeNumber, value, currentNode);
    }

    private static Node AddParentNode(string value, Node currentNode, uint nodeNumber)
    {
        var newChild = Node.CreateChildNode(nodeNumber, value, currentNode);
        currentNode.AddChildNode(newChild);

        return newChild;
    }
}
using MerkleTree.Nodes;

namespace MerkleTree;

internal class TreeBuilder
{
    private uint _nodeNumber = 1;
    private uint _leafNodeNumber;
    internal RootNode Root { get; private set; } = RootNode.CreateFirstRoot;
    private readonly NodeStack _parentNodesStack = new();

    public TreeBuilder()
    {
        _parentNodesStack.Push(Root);
    }

    internal (LeafNode leaf,RootNode root) AddNode(string value)
    {
        var currentNode = _parentNodesStack.Pop();

        if (currentNode == Node.None) throw new Exception();

        if (!currentNode.HasRoomForLeaf())
        {
            var newNode = CreateParentNode(value, currentNode, ++_nodeNumber);
            if (newNode.IsRoot()) Root=(newNode as RootNode)!;

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

        return (newLeaf,Root);
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

    private static RootNode CreateNewRootNode(string value, Node currentNode, uint nodeNumber)
    {
        return Node.CreateRootNode(nodeNumber, value, currentNode);
    }

    private static Node AddParentNode(string value, Node currentNode, uint nodeNumber)
    {
        var newChild = BranchNode.Create(nodeNumber, value, currentNode);
        currentNode.AddBranchNode(newChild);

        return newChild;
    }
}
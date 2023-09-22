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
            var newNode = CreateParentNode(value, currentNode);
            if (newNode.IsRoot()) Root=(newNode as RootNode)!;

            if (currentNode.HasRoomForNonLeaf() || currentNode.IsRoot())
                _parentNodesStack.Push(currentNode);
            currentNode = newNode;
        }

        var newLeaf = currentNode.AssignLeafNode( value);

        

        if (currentNode.HasRoom() || currentNode.IsRoot())
        {
            //Can not accept either leaf or non-leaf
            //we are done with this node. 
            //get next node from queue
            _parentNodesStack.Push(currentNode);
        }

        return (newLeaf,Root);
    }

    private static Node CreateParentNode(string value, Node node)
    {
        if (node.HasRoomForNonLeaf())
        {
            return AddParentNode(value, node);
        }

        //no room, create another root
        return CreateNewRootNode(value, node );

    }

    private static RootNode CreateNewRootNode(string value, Node currentNode)
    {
        return Node.CreateRootNode( value, currentNode);
    }

    private static Node AddParentNode(string value, Node currentNode)
    {
        var newChild = BranchNode.Create( value, currentNode);
        currentNode.AddBranchNode(newChild);

        return newChild;
    }
}
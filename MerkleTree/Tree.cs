namespace MerkleTree;

public class Tree
{
    public int LeafCount;
    public Node RootNode { get; private set; } = Node.NullNode;
    public Node CurrentParentNode { get; private set; } = Node.NullNode;
    private readonly NodeStack _parentNodesNodeStack = new();
    public int NodeNumber;


    public void AddNode(string value)
    {
        if (CurrentParentNode == Node.NullNode)
        {
            var newRoot = Node.CreateFirstRoot((++NodeNumber).ToString());
            RootNode = newRoot;
            CurrentParentNode = newRoot;
        }

        AddNode(value, _parentNodesNodeStack,CurrentParentNode);
        LeafCount++;
    }
    //todo: convert recursive calls to iterative
    private void AddNode(string value, NodeStack parentNodesStack, Node currentParentNode)
    {
        if (currentParentNode.HasRoom())
        {
            currentParentNode.Accept(value);
            CurrentParentNode = currentParentNode;
            return;
        }

        if (currentParentNode.CanHaveSubParent())
        {
            var newChild=Node.CreateChildNode((++NodeNumber).ToString(), currentParentNode);
            currentParentNode.AddChild(newChild);
            parentNodesStack.Push(currentParentNode);
            AddNode(value,parentNodesStack,newChild);
            return;
        }

        var parentInQueue = parentNodesStack.Pop();
        if (parentInQueue != Node.NullNode)
        {
            AddNode(value, parentNodesStack, parentInQueue);
            return;
        }

        Node newRoot =  Node.CreateRoot((++NodeNumber).ToString(), currentParentNode);
        AddNode(value, parentNodesStack, newRoot);
        RootNode = newRoot;
    }


    public void AddLeafNode(string nodeValue, Func<string, string> hashFunction)
    {
        var hashedValue = hashFunction.Invoke(nodeValue);
        LeafCount++;
    }
}
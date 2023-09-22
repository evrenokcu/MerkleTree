namespace MerkleTree.Api;

public interface ITree
{
    public void AddNode(string value);
    public NodeInformation GetLeafInformation(int leafNumber);
    public NodeInformation GetRootInformation();
}


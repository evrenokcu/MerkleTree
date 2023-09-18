﻿namespace MerkleTree.ConsoleApp;

public class LevelOrderVisitor:ITreeVisitor
{
    private readonly Action<Node> _action;
    private readonly Node _rootNode;

    public LevelOrderVisitor(Node rootNode,Action<Node> action)
    {
        _rootNode = rootNode;
        _action = action;
    }

    public void Visit()
    {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(_rootNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node != Node.NullNode)
            {
                _action(node);
                queue.Enqueue(node.Left);
                queue.Enqueue(node.Right);
            }
        }
    }
}
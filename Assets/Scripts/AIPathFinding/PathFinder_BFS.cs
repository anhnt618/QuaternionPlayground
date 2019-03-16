using System.Collections.Generic;
using UnityEngine;

public class PathFinder_BFS : MonoBehaviour
{
    [SerializeField] Node startNode, endNode;
    Dictionary<Vector2Int, Node> nodeBox = new Dictionary<Vector2Int, Node>();
    Queue<Node> queueNode = new Queue<Node>();
    List<Node> path = new List<Node>();
    bool isExplorering = true;
    Node centerNode;

    Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    private void Awake()
    {
        CreatePath();
    }

    

    private void LoadNodes()
    {
        var nodes = FindObjectsOfType<Node>();
        foreach (var node in nodes)
        {
            bool isOverLapped = nodeBox.ContainsKey(node.GetPosition());
            if (isOverLapped) { Debug.LogWarning("there is a node here already!!!"); }
            else
            {
                nodeBox.Add(node.GetPosition(), node);
            }
        }
    }

    private void BreadthFirstSearch()
    {
        queueNode.Enqueue(startNode);
        while (queueNode.Count > 0 && isExplorering)
        {
            centerNode = queueNode.Dequeue();
            centerNode.IsExplored = true;
            if (centerNode == endNode) { isExplorering = false; }
            ExploreNewNode();
        }
    }

    public List<Node> GetPath()
    {
        return path;
    }

    private void CreatePath()
    {
        LoadNodes();
        BreadthFirstSearch();
        path.Add(endNode);
        var previousNode = endNode.ExploredFrom;
        while (previousNode != startNode)
        {
            path.Add(previousNode);
            previousNode = previousNode.ExploredFrom;
        }
        path.Add(startNode);
        path.Reverse();
    }

    private void ExploreNewNode()
    {
        if (!isExplorering) { return; }
        foreach (var direction in directions)
        {
            var neighborNodePositon = centerNode.GetPosition() + direction;
            if (nodeBox.ContainsKey(neighborNodePositon))
            {
                QueueNeighborNode(neighborNodePositon);
            }
        }
    }

    private void QueueNeighborNode(Vector2Int neighborNodePositon)
    {
        Node neighborNode = nodeBox[neighborNodePositon];
        
        if (neighborNode.IsExplored == true || queueNode.Contains(neighborNode)) { return; } 
        else
        {
            queueNode.Enqueue(neighborNode);
            neighborNode.ExploredFrom = centerNode;
        }

        //if (!neighborNode.isExplored)                 // Note: very slow for some reason @@
        //{
        //    queueNode.Enqueue(neighborNode);
        //    neighborNode.ExploredFrom = centerNode;
        //}
    }
}

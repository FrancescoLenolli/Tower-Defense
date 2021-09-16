using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private MyGrid grid;

    public Pathfinding(MyGrid grid)
    {
        this.grid = grid;
    }

    public List<Node> GetPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.NodeFromWorldPoint(startPosition);
        Node targetNode = grid.NodeFromWorldPoint(targetPosition);

        return FindPath(startNode, targetNode);
    }

    public List<Node> GetPath(Node startNode, Node targetNode)
    {
        return FindPath(startNode, targetNode);
    }

    private List<Node> FindPath(Node start, Node end)
    {
        Node startNode = start;
        Node targetNode = end;

        Heap<Node> openSet = new Heap<Node>(grid.Size);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode, false))
            {
                if (!neighbour.isWalkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int movementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);

        path.Reverse();

        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public List<Node> path = new List<Node>();

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX;
    private int gridSizeY;

    public int Size { get => gridSizeX * gridSizeY; }
    public float NodeSize { get => nodeDiameter; }
    public int Width { get => gridSizeX; }
    public int Height { get => gridSizeY; }
    public Node[,] Grid { get => grid; }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid == null)
            return;

        foreach (Node node in grid)
        {
            Vector3 nodePosition = new Vector3(node.worldPosition.x, node.worldPosition.y - 0.5f, node.worldPosition.z);
            Gizmos.color = node.isWalkable ? Color.white : Color.red;
            Gizmos.DrawCube(nodePosition, Vector3.one * (nodeDiameter));
        }

        if (path == null || path.Count == 0)
            return;

        for (int i = 0; i < path.Count; ++i)
        {
            Vector3 position = new Vector3(path[i].worldPosition.x, path[i].worldPosition.y - 0.5f, path[i].worldPosition.z);
            Gizmos.color = i == 0 ? Color.black : i == path.Count - 1 ? Color.blue : Color.green;
            Gizmos.DrawCube(position, Vector3.one * (nodeDiameter));
        }
    }

    public void Init()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node, bool diagonalMovementAllowed)
    {
        List<Node> result = diagonalMovementAllowed ? GetNeighboursDiagonal(node) : GetNeighboursSidesOnly(node);
        result.RemoveAll(neighbour => neighbour == null);

        return result;
    }

    public void GetFirstAndLastPositions(out Vector3 firstPosition, out Vector3 lastPosition)
    {
        firstPosition = grid[0, 0].worldPosition;
        lastPosition = grid[gridSizeX - 1, gridSizeY - 1].worldPosition;
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        float boxSide = nodeRadius / 2;

        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int y = 0; y < gridSizeY; ++y)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool isWalkable = !(Physics.CheckBox(worldPoint, new Vector3(boxSide, 1, boxSide), Quaternion.identity, unwalkableMask));
                grid[x, y] = new Node(isWalkable, worldPoint, x, y);
            }
        }
    }

    private Node GetValidNode(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[x, y];
        }
        else return null;
    }

    private List<Node> GetNeighboursSidesOnly(Node node)
    {
        List<Node> neighbours = new List<Node>();

        int nodeX = node.gridX;
        int nodeY = node.gridY;

        int checkX = nodeX - 1;
        int checkY = nodeY;
        neighbours.Add(GetValidNode(checkX, checkY));

        checkX = nodeX + 1;
        checkY = nodeY;
        neighbours.Add(GetValidNode(checkX, checkY));

        checkX = nodeX;
        checkY = nodeY + 1;
        neighbours.Add(GetValidNode(checkX, checkY));

        checkX = nodeX;
        checkY = nodeY - 1;
        neighbours.Add(GetValidNode(checkX, checkY));

        return neighbours;
    }

    private List<Node> GetNeighboursDiagonal(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}

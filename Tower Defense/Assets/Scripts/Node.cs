using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public Node parent;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public int fCost => gCost + hCost;

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    private int heapIndex;
    private SpriteRenderer marker;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);

        if (compare == 0)
            compare = hCost.CompareTo(other.hCost);

        return -compare;
    }

    public void SetMarker(SpriteRenderer marker)
    {
        this.marker = marker;
    }

    public void SetMarkerColor(Color color)
    {
        if(marker)
        marker.color = color;
    }

    public void SetMarkerVisible(bool visible)
    {
        marker.gameObject.SetActive(visible);
    }

}

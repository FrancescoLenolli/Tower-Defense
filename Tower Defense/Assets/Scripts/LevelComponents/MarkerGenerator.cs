using System.Collections.Generic;
using UnityEngine;

public enum MarkerType { Standard, Path, Obstacle, Start, End }

public class MarkerGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform markersContainer = null;
    [SerializeField]
    private SpriteRenderer prefabMarker = null;
    [SerializeField]
    private Color standardColor = Color.white;
    [SerializeField]
    private Color pathColor = Color.white;
    [SerializeField]
    private Color startColor = Color.white;
    [SerializeField]
    private Color endColor = Color.white;

    private Color unwalkableColor = Color.clear;

    public void CreateMarkers(Node[,] grid, List<Node> path, Node startNode, Node endNode)
    {
        Color color;
        SpriteRenderer spriteRenderer;

        foreach (Node node in grid)
        {
            color = !(node.nodeState == Node.NodeState.Walkable) ? unwalkableColor : node == startNode ? startColor : node == endNode ? endColor : path.Contains(node) ? pathColor : standardColor;
            spriteRenderer = Instantiate(prefabMarker, new Vector3(node.worldPosition.x, 0.01f, node.worldPosition.z), prefabMarker.transform.rotation, markersContainer);
            spriteRenderer.color = color;

            node.SetMarker(spriteRenderer);
        }
    }

    public void ChangeMarker(Node node, MarkerType markerType)
    {
        if(node != null)
        node.SetMarkerColor(GetColor(markerType));
    }

    private Color GetColor(MarkerType markerType)
    {
        Color color = Color.black;

        switch (markerType)
        {
            case MarkerType.Path:
                color = pathColor;
                break;
            case MarkerType.Standard:
                color = standardColor;
                break;
            case MarkerType.Obstacle:
                color = Color.clear;
                break;
            case MarkerType.Start:
                color = startColor;
                break;
            case MarkerType.End:
                color = endColor;
                break;
            default:
                break;
        }

        return color;
    }
}

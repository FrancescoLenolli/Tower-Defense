using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private MyGrid grid = null;
    [SerializeField]
    private List<PlaceableObject> placeableObjects = new List<PlaceableObject>();

    private MouseInput mouseInput = null;
    private MarkerGenerator markerGenerator = null;
    private Pathfinding pathfinding = null;
    private List<Node> path = new List<Node>();
    private PlaceableObject placeableObject = null;
    private Node startNode = null;
    private Node endNode = null;
    private bool markersVisible = true;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
        markerGenerator = GetComponent<MarkerGenerator>();
    }

    private void Start()
    {
        grid.Init();
        pathfinding = new Pathfinding(grid);

        GenerateRandomPath();

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.SetPath(path);
        markerGenerator.CreateMarkers(grid.Grid, path, startNode, endNode);

        CameraControls cameraControls = FindObjectOfType<CameraControls>();
        Vector3 lowLimit;
        Vector3 highLimit;
        grid.GetFirstAndLastPositions(out lowLimit, out highLimit);
        cameraControls.SetCameraLimits(lowLimit, highLimit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            markersVisible = !markersVisible;

            foreach (Node node in grid.Grid)
            {
                node.SetMarkerVisible(markersVisible);
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            enemySpawner.SetPath(path);
            enemySpawner.StartWave(); // TODO: Replace with event when developing UI
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    GetObstacle(0);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    GetObstacle(1);
        //}
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }

        if (placeableObject)
        {
            placeableObject.transform.localPosition = grid.NodeFromWorldPoint(mouseInput.WorldPoint).worldPosition;
            placeableObject.transform.localPosition = new Vector3(placeableObject.transform.localPosition.x, 0.0f, placeableObject.transform.localPosition.z);
        }
    }

    public void GetObstacle(int index)
    {
        if (!placeableObject)
            placeableObject = Instantiate(placeableObjects[index], mouseInput.WorldPoint, Quaternion.identity, grid.transform);
    }

    private void GenerateRandomPath()
    {
        markerGenerator.ChangeMarker(this.startNode, MarkerType.Standard);
        markerGenerator.ChangeMarker(this.endNode, MarkerType.Standard);

        Node startNode = grid.Grid[0, UnityEngine.Random.Range(0, grid.Height)];
        Node endNode = grid.Grid[grid.Width - 1, UnityEngine.Random.Range(0, grid.Height)];

        this.startNode = startNode;
        this.endNode = endNode;

        GeneratePath(startNode.worldPosition, endNode.worldPosition);
    }

    private void GeneratePath(Vector3 startPosition, Vector3 endPosition)
    {
        List<Node> path = pathfinding.GetPath(startPosition, endPosition);
        SetPath(path);
    }

    private void SetPath(List<Node> newPath)
    {
        ChangePathMarkers(MarkerType.Standard);

        path.Clear();
        grid.path.Clear();
        path = newPath;
        grid.path = path;

        ChangePathMarkers(MarkerType.Path);
        markerGenerator.ChangeMarker(startNode, MarkerType.Start);
        markerGenerator.ChangeMarker(endNode, MarkerType.End);
    }

    private void PlaceObject()
    {
        if (!placeableObject)
            return;

        Node node = grid.NodeFromWorldPoint(placeableObject.transform.localPosition);

        bool isObjectAnObstacle = placeableObject.GetType() == typeof(Obstacle);
        bool isNodeValid;

        if(isObjectAnObstacle)
            isNodeValid = node != null && node.nodeState == Node.NodeState.Walkable && node != startNode && node != endNode;
        else
            isNodeValid = node != null && node.nodeState == Node.NodeState.ObstacleFree && node != startNode && node != endNode;

        if (!isNodeValid)
            return;


        node.nodeState = Node.NodeState.ObstacleFree;

        List<Node> newPath = pathfinding.GetPath(startNode, endNode);

        if (newPath == null)
        {
            node.nodeState = Node.NodeState.Walkable;
            return;
        }
        
        node.nodeState = isObjectAnObstacle ? Node.NodeState.ObstacleFree : Node.NodeState.ObstacleOccupied;

        placeableObject.Place();
        placeableObject = null;
        markerGenerator.ChangeMarker(node, MarkerType.Obstacle);

        if (path.Contains(node))
            SetPath(newPath);
    }

    private void ChangePathMarkers(MarkerType markerType)
    {
        for (int i = 1; i < path.Count - 1; ++i)
        {
            markerGenerator.ChangeMarker(path[i], markerType);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private MyGrid grid = null;
    [SerializeField]
    private PlaceableObject obstaclePrefab = null;
    [SerializeField]
    private PlaceableObject turretPrefab = null;

    private MouseInput mouseInput = null;
    private MarkerGenerator markerGenerator = null;
    private Pathfinding pathfinding = null;
    private List<Node> path = new List<Node>();
    private bool autogeneratePath = false;
    private PlaceableObject obstacle = null;
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
        StartCoroutine(AutoGeneratePath());

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        //enemySpawner.SetPath(path);
        markerGenerator.CreateMarkers(grid.Grid, path, startNode, endNode);

        CameraControls cameraControls = FindObjectOfType<CameraControls>();
        Vector3 lowLimit;
        Vector3 highLimit;
        grid.GetFirstAndLastPositions(out lowLimit, out highLimit);
        cameraControls.SetCameraLimits(lowLimit, highLimit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            autogeneratePath = !autogeneratePath;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenerateRandomPath();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            markersVisible = !markersVisible;

            foreach (Node node in grid.Grid)
            {
                node.SetMarkerVisible(markersVisible);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetObstacle(obstaclePrefab);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetObstacle(turretPrefab);
        }
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObstacle();
        }

        if (obstacle)
        {
            obstacle.transform.localPosition = grid.NodeFromWorldPoint(mouseInput.WorldPoint).worldPosition;
            obstacle.transform.localPosition = new Vector3(obstacle.transform.localPosition.x, 0.0f, obstacle.transform.localPosition.z);
        }
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

    private void GetObstacle(PlaceableObject prefab)
    {
        if (!obstacle)
            obstacle = Instantiate(prefab, mouseInput.WorldPoint, Quaternion.identity, grid.transform);
    }

    private void PlaceObstacle()
    {
        if (!obstacle)
            return;

        Node node = grid.NodeFromWorldPoint(obstacle.transform.localPosition);

        if (node == null || !node.isWalkable || node == startNode || node == endNode)
            return;

        node.isWalkable = false;

        List<Node> newPath = pathfinding.GetPath(startNode, endNode);

        if (newPath == null)
        {
            node.isWalkable = true;
            return;
        }

        obstacle.Place();
        obstacle = null;
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

    private IEnumerator AutoGeneratePath()
    {
        while (true)
        {
            if (autogeneratePath)
            {
                GenerateRandomPath();
                yield return new WaitForSeconds(0.01f);
            }

            yield return null;
        }
    }
}

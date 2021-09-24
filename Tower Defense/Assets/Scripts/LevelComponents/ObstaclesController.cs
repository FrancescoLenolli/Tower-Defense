using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    [SerializeField]
    private List<PlaceableObjectInfo> placeableObjects = new List<PlaceableObjectInfo>();

    private PlaceableObject placeableObject = null;
    private MyGrid grid;
    private MouseInput mouseInput;

    public List<PlaceableObjectInfo> PlaceableObjects { get => placeableObjects; }

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
        placeableObjects = Resources.LoadAll<PlaceableObjectInfo>("").ToList();
    }

    private void Update()
    {
        if (placeableObject)
        {
            placeableObject.transform.localPosition = grid.NodeFromWorldPoint(mouseInput.WorldPoint).worldPosition;
            placeableObject.transform.localPosition = new Vector3(placeableObject.transform.localPosition.x, 0.0f, placeableObject.transform.localPosition.z);
        }
    }

    public void InitData(MyGrid grid)
    {
        this.grid = grid;
    }

    public PlaceableObject GetPlaceableObject()
    {
        return placeableObject;
    }

    public void ResetPlaceableObject()
    {
        placeableObject = null;
    }

    public void GetObstacle(int index)
    {
        if (!placeableObject)
            placeableObject = Instantiate(placeableObjects[index].placeableObject, mouseInput.WorldPoint, Quaternion.identity, grid.transform);
    }
}

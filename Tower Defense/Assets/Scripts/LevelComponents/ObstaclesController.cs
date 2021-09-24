using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    [SerializeField]
    private List<PlaceableObject> placeableObjects = new List<PlaceableObject>();

    private PlaceableObject placeableObject = null;
    private MyGrid grid;
    private MouseInput mouseInput;

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
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
            placeableObject = Instantiate(placeableObjects[index], mouseInput.WorldPoint, Quaternion.identity, grid.transform);
    }
}

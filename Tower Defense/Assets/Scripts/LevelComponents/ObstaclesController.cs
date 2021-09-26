using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclesController : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableMask;

    private List<PlaceableObjectInfo> placeableObjects = new List<PlaceableObjectInfo>();
    private PlaceableObject placeableObject = null;
    private MyGrid grid;
    private MouseInput mouseInput;

    public List<PlaceableObjectInfo> PlaceableObjects { get => placeableObjects; }

    private void Awake()
    {
        mouseInput = GetComponent<MouseInput>();
        mouseInput.InitData(selectableMask);
        placeableObjects = Resources.LoadAll<PlaceableObjectInfo>("").ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && placeableObject)
        {
            ResetCurrentObject();
        }

        if (placeableObject)
        {
            MoveCurrentObject();
        }
    }

    public void InitData(MyGrid grid)
    {
        this.grid = grid;
    }

    public void SetPlaceableObject(PlaceableObject placeableObject)
    {
        this.placeableObject = placeableObject;
    }

    public PlaceableObject GetPlaceableObject()
    {
        return placeableObject;
    }

    public void ReplaceCurrentObject()
    {
        placeableObject = null;
    }

    public void GetObstacle(int index)
    {
        if (placeableObject)
            ResetCurrentObject();

        if (!placeableObject)
        {
            placeableObject = Instantiate(placeableObjects[index].placeableObject, mouseInput.WorldPoint, Quaternion.identity, grid.transform);
        }
    }

    public PlaceableObject RepositionObject()
    {
        if (placeableObject)
            return null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 50, selectableMask))
        {
            PlaceableObject placeableObject = raycastHit.transform.GetComponent<PlaceableObject>();
            if (placeableObject)
            {
                return placeableObject;
            }
        }

        return null;
    }

    private void ResetCurrentObject()
    {
        Destroy(placeableObject.gameObject);
        placeableObject = null;
    }

    private void MoveCurrentObject()
    {
        placeableObject.transform.localPosition = grid.NodeFromWorldPoint(mouseInput.WorldPoint).worldPosition;
        placeableObject.transform.localPosition = new Vector3(placeableObject.transform.localPosition.x, 0.0f, placeableObject.transform.localPosition.z);
    }
}

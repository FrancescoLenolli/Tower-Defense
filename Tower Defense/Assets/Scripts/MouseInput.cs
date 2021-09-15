using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableMask;
    private Vector3 worldPoint = Vector3.zero;

    public Vector3 WorldPoint { get => worldPoint; }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 50, selectableMask))
        {
            worldPoint = new Vector3(raycastHit.point.x, 0.0f, raycastHit.point.z);
        }
    }
}

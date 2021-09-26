using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private LayerMask selectableMask;
    private Vector3 worldPoint = Vector3.zero;

    public Vector3 WorldPoint { get => worldPoint; }

    private void Update()
    {
        MouseRaycast();  
    }

    public void InitData(LayerMask selectableMask)
    {
        this.selectableMask = selectableMask;
    }

    private void MouseRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 50, selectableMask))
        {
            SetWorldPoint(raycastHit.point);
        }
    }

    private void SetWorldPoint(Vector3 point)
    {
        worldPoint = new Vector3(point.x, 0.0f, point.z);
    }
}

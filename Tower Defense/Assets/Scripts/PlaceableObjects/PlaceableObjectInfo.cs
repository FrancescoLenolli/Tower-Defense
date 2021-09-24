using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Custom/PlaceableObject", fileName = "NewPlaceableObject")]
public class PlaceableObjectInfo : ScriptableObject
{
    public new string name = "PlaceableObject";
    public int cost = 100;
    public Sprite placeableIcon = null;
    public PlaceableObject placeableObject = null;
}

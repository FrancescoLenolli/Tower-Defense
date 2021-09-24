using System;
using System.Collections.Generic;
using TMPro;
using UIFramework.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIView_HUD_Main : UIView
{
    [SerializeField]
    private GameObject prefabPlaceableObjectButton = null;
    [SerializeField]
    private Transform buttonsParent = null;

    private Action<int> onSpawnObstacle;

    public Action<int> OnSpawnObstacle { get => onSpawnObstacle; set => onSpawnObstacle = value; }

    public void InitButtons(List<PlaceableObjectInfo> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            int index = i;
            UnityAction spawnObstacle = () => onSpawnObstacle?.Invoke(index);
            GameObject placeableObjectButton = Instantiate(prefabPlaceableObjectButton, buttonsParent);
            placeableObjectButton.GetComponent<Button>().onClick.AddListener(spawnObstacle);
            placeableObjectButton.GetComponentInChildren<TextMeshProUGUI>().text = list[i].name;
        }
    }
}

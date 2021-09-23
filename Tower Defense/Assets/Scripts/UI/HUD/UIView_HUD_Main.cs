using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework.StateMachine;
using UnityEngine;

public class UIView_HUD_Main : UIView
{
    private Action<int> onSpawnObstacle;

    public Action<int> OnSpawnObstacle { get => onSpawnObstacle; set => onSpawnObstacle = value; }

    public void SpawnObstacle(int index)
    {
       onSpawnObstacle?.Invoke(index);
    }
}

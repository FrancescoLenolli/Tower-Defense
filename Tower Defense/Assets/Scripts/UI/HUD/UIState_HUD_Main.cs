using System.Collections;
using System.Collections.Generic;
using UIFramework.StateMachine;
using UnityEngine;

public class UIState_HUD_Main : UIState_HUD
{
    private UIView_HUD_Main view;
    private Level level;

    public override void PrepareState(UIStateMachine owner)
    {
        base.PrepareState(owner);

        view = root.HUDMain;
        level = FindObjectOfType<Level>();

        view.OnSpawnObstacle += SpawnObstacle;
        view.InitButtons(level.ObstaclesController.PlaceableObjects);
    }

    public override void ShowState()
    {
        view.ShowView();
    }

    public override void HideState()
    {
        view.HideView();
    }

    private void SpawnObstacle(int index)
    {
        Debug.Log(index);

        level.ObstaclesController.GetObstacle(index);
    }
}

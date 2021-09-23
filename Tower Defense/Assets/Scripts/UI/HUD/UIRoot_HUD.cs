using System.Collections;
using System.Collections.Generic;
using UIFramework.StateMachine;
using UnityEngine;

public class UIRoot_HUD : UIRoot
{
    [SerializeField]
    private UIView_HUD_Main hudMain;

    public UIView_HUD_Main HUDMain { get => hudMain; set => hudMain = value; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLostScreen : Screen
{
    public event UnityAction RestartButtonClick;

    protected override void OnButtonClick()
    {
        RestartButtonClick?.Invoke();
    }
}

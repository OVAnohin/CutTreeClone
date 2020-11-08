using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextLevelScreen : Screen
{
    public event UnityAction NextLevelButtonClick;

    protected override void OnButtonClick()
    {
        NextLevelButtonClick?.Invoke();
    }
}

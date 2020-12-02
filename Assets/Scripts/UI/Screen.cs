using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Screen : MonoBehaviour
{
    [SerializeField] protected Button Button;
    [SerializeField] protected TMP_Text CurrentLevel;
    [SerializeField] private Button _exit;

    private void OnEnable()
    {
        Button.onClick.AddListener(OnButtonClick);
        _exit.onClick.AddListener(OnExitClick);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnButtonClick);
        _exit.onClick.RemoveListener(OnExitClick);
    }

    private void OnExitClick()
    {
        Application.Quit();
    }

    public void SetLevel(string level)
    {
        CurrentLevel.text = level;
    }

    protected abstract void OnButtonClick();
}

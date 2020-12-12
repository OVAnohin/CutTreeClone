using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LeafChecker))]
public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private LeafChecker _leafChecker;

    private void Awake()
    {
        _leafChecker = GetComponent<LeafChecker>();
        _slider.value = 0;
    }

    private void OnEnable()
    {
        _leafChecker.YellowLeavesCutted += OnValueChanged;
    }

    private void OnDisable()
    {
        _leafChecker.YellowLeavesCutted -= OnValueChanged;
    }

    public void OnValueChanged(int value, float maxValue)
    {
        _slider.value = value / maxValue;
    }
}

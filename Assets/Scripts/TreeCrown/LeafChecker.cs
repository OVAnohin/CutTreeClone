using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TreeCrownFiller))]
public class LeafChecker : MonoBehaviour
{
    [SerializeField] private int _percentageOfLosses;
    [SerializeField] private int _percentageOfWinnig;

    public event UnityAction GameLevelLost;
    public event UnityAction GameLevelWin;
    public event UnityAction<int, float> YellowLeavesCutted;

    private List<GameObject> _greenLeaves;
    private List<GameObject> _yellowLeaves;
    private float _greenLeavesHundredPercent;
    private float _yellowLeavesHundredPercent;
    private bool _isShowWarning;

    public void ResetChecker()
    {
        StartCoroutine(WaitTreeCrownFiller());
        CountYellowLeavesCutted();
    }

    private void OnLeafClipped(TreeLeaf leaf)
    {
        leaf.Clipped -= OnLeafClipped;

        if (leaf.GetType().ToString() == "GreenLeaf" && _isShowWarning == false)
            StartCoroutine(ShowGreenZoneWarning(_greenLeaves));

        if (leaf.GetType().ToString() == "YellowLeaf")
            CountYellowLeavesCutted();

        if (CheckClippedStatus(_greenLeaves, _greenLeavesHundredPercent, _percentageOfLosses))
        {
            UnSetEvent();
            GameLevelLost?.Invoke();
        }
        else if (CheckClippedStatus(_yellowLeaves, _yellowLeavesHundredPercent, _percentageOfWinnig))
        {
            UnSetEvent();
            GameLevelWin?.Invoke();
        }
    }

    private void CountYellowLeavesCutted()
    {
        float maxValue = (_yellowLeavesHundredPercent * _percentageOfWinnig) / 100;
        int value = Convert.ToInt32(_yellowLeavesHundredPercent) - NumberLeavesOnTree(_yellowLeaves);
        YellowLeavesCutted?.Invoke(value, maxValue);
    }

    private bool CheckClippedStatus(List<GameObject> treeLeaves, float hundredPercent, int percent)
    {
        int count = NumberLeavesOnTree(treeLeaves);
        float currentPercent = (count * 100) / hundredPercent;

        if (100 - currentPercent >= percent)
            return true;

        return false;
    }

    private IEnumerator WaitTreeCrownFiller()
    {
        bool isInitComplete = GetComponent<TreeCrownFiller>().IsInitComplete;
        while (isInitComplete == false)
        {
            yield return new WaitForSeconds(1);
        }

        _greenLeaves = GetComponent<TreeCrownFiller>().GreenLeaves;
        _yellowLeaves = GetComponent<TreeCrownFiller>().YellowLeaves;

        InitLeaf(_greenLeaves, out _greenLeavesHundredPercent);
        InitLeaf(_yellowLeaves, out _yellowLeavesHundredPercent);
    }

    private IEnumerator ShowGreenZoneWarning(List<GameObject> treeLeaves)
    {
        _isShowWarning = true;
        Color color = treeLeaves[0].GetComponent<SpriteRenderer>().color;

        SetColorToLeaves(treeLeaves, Color.red);

        yield return new WaitForSeconds(.2f);

        SetColorToLeaves(treeLeaves, color);
        _isShowWarning = false;
    }

    private void SetColorToLeaves(List<GameObject> treeLeaves, Color color)
    {
        foreach (GameObject leaf in treeLeaves)
            leaf.GetComponent<SpriteRenderer>().color = color;
    }

    private void InitLeaf(List<GameObject> treeLeaves, out float hundredPercent)
    {
        foreach (GameObject leaf in treeLeaves)
            leaf.GetComponent<TreeLeaf>().Clipped += OnLeafClipped;

        hundredPercent = treeLeaves.Count(s => s.activeSelf == true);
    }

    private void UnSetEvent()
    {
        foreach (GameObject leaf in _greenLeaves)
            leaf.GetComponent<TreeLeaf>().Clipped -= OnLeafClipped;

        foreach (GameObject leaf in _yellowLeaves)
            leaf.GetComponent<TreeLeaf>().Clipped -= OnLeafClipped;
    }

    private int NumberLeavesOnTree(List<GameObject> treeLeaves)
    {
        return treeLeaves.Count(s => s.activeSelf == true);
    }
}

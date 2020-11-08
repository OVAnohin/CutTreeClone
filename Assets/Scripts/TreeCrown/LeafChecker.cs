﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TreeCrownFiller))]
public class LeafChecker : MonoBehaviour
{
    [SerializeField] private int _percentageOfLosses;
    [SerializeField] private int _percentageOfWinnig;

    public event UnityAction GameLevelLost;
    public event UnityAction GameLevelWin;

    private List<TreeLeaf> _greenLeaves;
    private List<TreeLeaf> _yellowLeaves;
    private float _greenLeavesHundredPercent;
    private float _yellowLeavesHundredPercent;

    public void ResetChecker()
    {
        StartCoroutine(WaitTreeCrownFiller());
    }

    private void OnLeafClipped(TreeLeaf leaf)
    {
        string leafType = leaf.GetType().ToString();
        switch (leafType)
        {
            case "GreenLeaf":
                _greenLeaves.Remove(leaf);
                break;
            case "YellowLeaf":
                _yellowLeaves.Remove(leaf);
                break;
        }

        leaf.Clipped -= OnLeafClipped;

        if (CheckClippedStatus(_greenLeaves, _greenLeavesHundredPercent, _percentageOfLosses))
        {
            Debug.Log("Lost");
            GameLevelLost?.Invoke();
        }

        if (CheckClippedStatus(_yellowLeaves, _yellowLeavesHundredPercent, _percentageOfWinnig))
        {
            Debug.Log("Win");
            GameLevelWin?.Invoke();
        }
    }

    private bool CheckClippedStatus(List<TreeLeaf> leaves, float hundredPercent, int percent)
    {
        float currentPercent = (leaves.Count * 100) / hundredPercent;
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

    private void InitLeaf(List<TreeLeaf> treeLeaves, out float hundredPercent)
    {
        foreach (TreeLeaf leaf in treeLeaves)
            leaf.Clipped += OnLeafClipped;

        hundredPercent = treeLeaves.Count;
    }
}

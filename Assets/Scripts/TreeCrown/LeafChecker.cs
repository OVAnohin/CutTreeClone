using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TreeCrownFiller))]
public class LeafChecker : MonoBehaviour
{
    [SerializeField] private int _percentageOfLosses;

    private List<TreeLeaf> _greenLeaves;
    private List<TreeLeaf> _yellowLeaves;
    private float _greenLeavesHundredPercent;
    private float _yellowLeavesHundredPercent;

    private void OnEnable()
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

        float currentPercent = (_greenLeaves.Count * 100) / _greenLeavesHundredPercent;
        if (100 - currentPercent >= _percentageOfLosses)
            Debug.Log("Loose");

        currentPercent = CheckClippedStatus();
    }

    private bool CheckClippedStatus(List<TreeLeaf> _yellowLeaves, float hundredPercent)
    {
        float currentPercent = (_yellowLeaves.Count * 100) / _yellowLeavesHundredPercent;
        if (100 - currentPercent >= 90)
            Debug.Log("Win");
        return currentPercent;
    }

    private IEnumerator WaitTreeCrownFiller()
    {
        while (_greenLeaves == null && _yellowLeaves == null)
        {
            _greenLeaves = GetComponent<TreeCrownFiller>().GreenLeaves;
            _yellowLeaves = GetComponent<TreeCrownFiller>().YellowLeaves;
            yield return new WaitForSeconds(1);
        }

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

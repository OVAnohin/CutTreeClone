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
        leaf.Clipped -= OnLeafClipped;

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

    private bool CheckClippedStatus(List<TreeLeaf> treeLeaves, float hundredPercent, int percent)
    {
        int count = treeLeaves.Count(s => s.gameObject.activeSelf == true);
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

    private void InitLeaf(List<TreeLeaf> treeLeaves, out float hundredPercent)
    {
        foreach (TreeLeaf leaf in treeLeaves)
            leaf.Clipped += OnLeafClipped;

        hundredPercent = treeLeaves.Count(s => s.gameObject.activeSelf == true);
    }

    private void UnSetEvent()
    {
        foreach (TreeLeaf leaf in _greenLeaves)
            leaf.Clipped -= OnLeafClipped;

        foreach (TreeLeaf leaf in _yellowLeaves)
            leaf.Clipped -= OnLeafClipped;
    }
}

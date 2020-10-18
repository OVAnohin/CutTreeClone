using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCrownFiller : MonoBehaviour
{
    [SerializeField] private TreeLeave _treeLeave;
    [SerializeField] private Transform _treeCrown;
    [SerializeField] private int _crownWidth;
    [SerializeField] private int _crownHeight;
    [SerializeField] private float _stepPlacementLeaves;

    private void Start()
    {
        FillCrownOfTree();
    }

    private void FillCrownOfTree()
    {
        float stepX = 0, stepY = 0;

        for (int i = 0; i < _crownHeight; i++)
        {
            stepX = 0;

            for (int j = 0; j < _crownWidth; j++)
            {
                Vector3 position = new Vector3(_treeCrown.position.x + stepX, _treeCrown.position.y + stepY, 0);
                TreeLeave treeLeave = Instantiate(_treeLeave, position, Quaternion.identity, _treeCrown);
                stepX += _stepPlacementLeaves;
            }

            stepY += _stepPlacementLeaves;
        }
    }
}

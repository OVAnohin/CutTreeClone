using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeCrownFiller : MonoBehaviour
{
    [SerializeField] private GameObject _grid;
    [SerializeField] private Tilemap[] _levelMaps;
    [SerializeField] private Tilemap _level;
    [SerializeField] private TreeLeaf _greenLeaf;
    [SerializeField] private TreeLeaf _yellowLeaf;
    [SerializeField] private Transform _treeCrown;

    public List<TreeLeaf> GreenLeaves { get; private set; }
    public List<TreeLeaf> YellowLeaves { get; private set; }
    public bool IsInitComplete { get; private set; }

    private System.Random _random = new System.Random();
    private int _numberLevel = 0;

    private void Awake()
    {
        GreenLeaves = new List<TreeLeaf>();
        YellowLeaves = new List<TreeLeaf>();
    }

    public void ReFillCrown()
    {
        IsInitComplete = false;

        ResetLists();

        GreenLeaves = new List<TreeLeaf>();
        YellowLeaves = new List<TreeLeaf>();

        _grid.SetActive(true);
        CloneLevelToCurrentLevel();
        FillCrownOfTree();
        _grid.SetActive(false);

        IsInitComplete = true;
    }

    public void NextLevel()
    {
        if (_numberLevel < _levelMaps.Length - 1)
            _numberLevel++;
        else
            _numberLevel = 0;
    }

    private void ResetLists()
    {
        foreach (TreeLeaf item in GreenLeaves)
            Destroy(item.gameObject);

        foreach (TreeLeaf item in YellowLeaves)
            Destroy(item.gameObject);
    }

    private void CloneLevelToCurrentLevel()
    {
        _level.ClearAllTiles();
        for (int y = _levelMaps[_numberLevel].cellBounds.y; y <= _levelMaps[_numberLevel].cellBounds.size.y; y++)
        {
            for (int x = _levelMaps[_numberLevel].cellBounds.x; x <= _levelMaps[_numberLevel].cellBounds.size.x; x++)
            {
                TileBase tile = _levelMaps[_numberLevel].GetTile(new Vector3Int(x, y, 0));

                if (tile != null)
                    _level.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private void FillCrownOfTree()
    {
        for (int y = _level.cellBounds.y; y <= _level.cellBounds.size.y; y++)
        {
            for (int x = _level.cellBounds.x; x <= _level.cellBounds.size.x; x++)
            {
                TileBase tile = _level.GetTile(new Vector3Int(x, y, 0));

                if (tile != null)
                {
                    switch (tile.name)
                    {
                        case "GreenCircle":
                            GenerateLeaves(y, x, _greenLeaf);
                            break;
                        case "YellowCircle":
                            GenerateLeaves(y, x, _yellowLeaf);
                            break;
                    }

                    _level.SetTile(new Vector3Int(x, y, 0), default);
                }
            }
        }
    }

    private void GenerateLeaves(int y, int x, TreeLeaf leaf)
    {
        for (int i = 0; i < 4; i++)
        {
            float step = (float)_random.Next(-1, 1) / 10f;
            Vector3 vector3 = _level.GetBoundsLocal(new Vector3Int(x, y, 0)).center + new Vector3(step, step, 0);
            TreeLeaf spawned = Instantiate(leaf, vector3, Quaternion.Euler(0, 0, _random.Next(0, 360)), _treeCrown);

            AddLeafToList(leaf, spawned);
        }
    }

    private void AddLeafToList(TreeLeaf leaf, TreeLeaf spawned)
    {
        string leafType = leaf.GetType().ToString();
        switch (leafType)
        {
            case "GreenLeaf":
                GreenLeaves.Add(spawned);
                break;
            case "YellowLeaf":
                YellowLeaves.Add(spawned);
                break;
        }
    }
}

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
    [SerializeField] private int _capacity;

    public List<TreeLeaf> GreenLeaves { get; private set; }
    public List<TreeLeaf> YellowLeaves { get; private set; }
    public bool IsInitComplete { get; private set; }

    private System.Random _random = new System.Random();
    private int _numberLevel = 0;

    private void Awake()
    {
        GreenLeaves = new List<TreeLeaf>();
        YellowLeaves = new List<TreeLeaf>();

        Initialize(GreenLeaves, _greenLeaf);
        Initialize(YellowLeaves, _yellowLeaf);
    }

    public void ReFillCrown()
    {
        IsInitComplete = false;
        DropLeaves();

        _grid.SetActive(true);
        CloneLevelToCurrentLevel();
        FillCrownOfTree();
        _grid.SetActive(false);

        IsInitComplete = true;
    }

    public void DropLeaves()
    {
        ResetLists(YellowLeaves);
        ResetLists(GreenLeaves);
    }

    public void NextLevel()
    {
        if (_numberLevel < _levelMaps.Length - 1)
            _numberLevel++;
        else
            _numberLevel = 0;
    }

    public void DropYellowLeaves()
    {
        ResetLists(YellowLeaves);
    }

    private void Initialize(List<TreeLeaf> treeLeaves, TreeLeaf prefab)
    {
        for (int i = 0; i < _capacity; i++)
        {
            TreeLeaf spawned = Instantiate(prefab, _treeCrown.transform);
            spawned.gameObject.SetActive(false);

            treeLeaves.Add(spawned);
        }
    }

    private void ResetLists(List<TreeLeaf> treeLeaves)
    {
        foreach (TreeLeaf item in treeLeaves)
            item.gameObject.SetActive(false);
    }

    protected bool TryGetLeaf(List<TreeLeaf> treeLeaves, out TreeLeaf result)
    {
        result = treeLeaves.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result != null;
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
                            GenerateLeaves(y, x, GreenLeaves);
                            break;
                        case "YellowCircle":
                            GenerateLeaves(y, x, YellowLeaves);
                            break;
                    }

                    _level.SetTile(new Vector3Int(x, y, 0), default);
                }
            }
        }
    }

    private void GenerateLeaves(int y, int x, List<TreeLeaf> treeLeaves)
    {
        for (int i = 0; i < 4; i++)
        {
            TreeLeaf leaf;
            if (TryGetLeaf(treeLeaves, out leaf))
            {
                float step = (float)_random.Next(-1, 1) / 10f;
                Vector3 spawnPoint = _level.GetBoundsLocal(new Vector3Int(x, y, 0)).center + new Vector3(step, step, 0);
                leaf.gameObject.SetActive(true);
                leaf.gameObject.transform.position = spawnPoint;
                leaf.gameObject.transform.rotation = Quaternion.Euler(0, 0, _random.Next(0, 360));
            }
        }
    }
}

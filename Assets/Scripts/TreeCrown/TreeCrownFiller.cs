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
    [SerializeField] private GameObject _greenLeaf;
    [SerializeField] private GameObject _yellowLeaf;
    [SerializeField] private GameObject _coin;
    [SerializeField] private Transform _treeCrown;

    public List<GameObject> GreenLeaves { get; private set; }
    public List<GameObject> YellowLeaves { get; private set; }
    public bool IsInitComplete { get; private set; }
    public string CurrentLevel => _levelMaps[_numberLevel].name;

    private System.Random _random = new System.Random();
    private int _numberLevel = 0;
    private int _leavesCapacity = 1500;
    private int _coinsCapacity = 3;
    private List<GameObject> _coins = new List<GameObject>();

    private void Awake()
    {
        GreenLeaves = new List<GameObject>();
        YellowLeaves = new List<GameObject>();

        Initialize(GreenLeaves, _greenLeaf, _leavesCapacity);
        Initialize(YellowLeaves, _yellowLeaf, _leavesCapacity);
        Initialize(_coins, _coin, _coinsCapacity);
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
        ResetLists(_coins);
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

    private void Initialize<T>(List<T> list, T prefab, int capacity) where T : class
    {
        for (int i = 0; i < capacity; i++)
        {
            GameObject spawned = Instantiate(prefab as GameObject, _treeCrown.transform);
            spawned.SetActive(false);
            list.Add(spawned as T);
        }
    }

    private void ResetLists(List<GameObject> treeLeaves)
    {
        foreach (GameObject item in treeLeaves)
            item.SetActive(false);
    }

    protected bool TryGetElementFromList(List<GameObject> treeLeaves, out GameObject result)
    {
        result = treeLeaves.FirstOrDefault(p => p.activeSelf == false);

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
                            GenerateElement(y, x, GreenLeaves, 4);
                            break;
                        case "YellowCircle":
                            GenerateElement(y, x, YellowLeaves, 4);
                            break;
                        case "WhiteCircle":
                            GenerateElement(y, x, _coins, 1);
                            break;
                    }

                    _level.SetTile(new Vector3Int(x, y, 0), default);
                }
            }
        }
    }

    private void GenerateElement(int y, int x, List<GameObject> list, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject element;
            if (TryGetElementFromList(list, out element))
            {
                float step;
                if (count > 1)
                    step = (float)_random.Next(-1, 1) / 10f;
                else
                    step = 0;

                Vector3 spawnPoint = _level.GetBoundsLocal(new Vector3Int(x, y, 0)).center + new Vector3(step, step, 0);
                element.SetActive(true);
                element.transform.position = spawnPoint;
                element.transform.rotation = Quaternion.Euler(0, 0, _random.Next(0, 360));
            }
        }
    }
}

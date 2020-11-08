using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeCrownFiller : MonoBehaviour
{
    [SerializeField] private Tilemap[] _levelMap;
    [SerializeField] private TreeLeaf _greenLeaf;
    [SerializeField] private TreeLeaf _yellowLeaf;
    [SerializeField] private Transform _treeCrown;

    public List<TreeLeaf> GreenLeaves { get; private set; }
    public List<TreeLeaf> YellowLeaves { get; private set; }

    private System.Random _random = new System.Random();
    private Tilemap _level;

    private void Awake()
    {
        _level = _levelMap[0];
        GreenLeaves = new List<TreeLeaf>();
        YellowLeaves = new List<TreeLeaf>();
        FillCrownOfTree();
    }

    public void ReFillCrown()
    {
        _level = _levelMap[0].MemberwiseClone() as Tilemap;
        //MyDerivedClass clone = original.MemberwiseClone() as MyDerivedClass;
        //CultureInfo[] arrCIClone = (CultureInfo[])arrCI.Clone();
        FillCrownOfTree();
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

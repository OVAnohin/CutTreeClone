using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Filler : MonoBehaviour
{
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private TileBase _blueTile;
    [SerializeField] private TileBase _redTile;
    // [SerializeField] private Transform _pointer;
    [SerializeField] private GameObject _template;
    [SerializeField] private float _step;

    private void Start()
    {
        //Vector3Int cellPosition = _tileMap.WorldToCell(transform.position);
        //Instantiate(_template, cellPosition, Quaternion.identity, transform);
        Debug.Log(_tileMap.cellBounds);
        //Debug.Log(_tileMap.GetTile(new Vector3Int(-3, -5, 0)));

        for (int y = -7; y < 8; y++)
        {
            for (int x = -10; x < 0; x++)
            {
                TileBase tileBase = _tileMap.GetTile(new Vector3Int(x, y, 0));
                if (tileBase != null)
                {
                    Debug.Log(tileBase.name);
                    if (tileBase.name == "TileBlue")
                    {
                        Debug.Log(_tileMap.GetBoundsLocal(new Vector3Int(x, y, 0)));
                        Vector3 vector3 = _tileMap.GetBoundsLocal(new Vector3Int(x, y, 0)).center + new Vector3(0.1f, 0.1f, 0);
                        Instantiate(_template, vector3, Quaternion.identity, transform);
                    }
                }
            }
        }




        float stepX = 0;
        float stepY = 0;
        for (int y = 1; y <= 12; y++)
        {
            stepX = 0;

            for (int x = 1; x <= 22; x++)
            {
                Vector3 position = new Vector3(transform.position.x + stepX, transform.position.y + stepY, 0);

                //TileBase tileBase = _tileMap.GetTile(new Vector3Int((int)position.x, (int)position.y, 0));
                //if (tileBase != null)
                //{
                //    Debug.Log(tileBase.name);
                //}

                GameObject spawned = Instantiate(_template, position, Quaternion.identity, transform);
                stepX += _step;
            }

            stepY += _step;
        }

    }
}

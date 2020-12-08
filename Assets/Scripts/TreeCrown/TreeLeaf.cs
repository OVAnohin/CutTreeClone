using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TreeLeaf : MonoBehaviour
{
    [SerializeField] private GameObject _dropEffect;

    public event UnityAction<TreeLeaf> Clipped;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Scissors>())
        {
            Clipped?.Invoke(this);

            ShowDrop();

            gameObject.SetActive(false);
        }
    }

    public void ShowDrop()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Instantiate(_dropEffect, pos, Quaternion.identity);
    }
}

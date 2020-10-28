using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TreeLeaf : MonoBehaviour
{
    public event UnityAction<TreeLeaf> Clipped;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Scissors>())
        {
            Clipped?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

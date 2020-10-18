using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLeave : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Scissors>())
            Destroy(gameObject);
    }
}

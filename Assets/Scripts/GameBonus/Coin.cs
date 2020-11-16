using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Scissors>() != null)
        {
            collision.GetComponent<Scissors>().AddCoin();
            gameObject.SetActive(false);
        }
    }
}

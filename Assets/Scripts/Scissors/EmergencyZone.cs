using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyZone : MonoBehaviour
{
    [SerializeField] private Scissors _scissors;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TreeLeaf>())
            _scissors.TryPlay();
    }
}

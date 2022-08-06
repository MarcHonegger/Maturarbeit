using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6)
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<TroopHandler>();
        enemy.TakeDamage(50);
    }
}

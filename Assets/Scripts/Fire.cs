using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Fire : NetworkBehaviour
{
    [Server]
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

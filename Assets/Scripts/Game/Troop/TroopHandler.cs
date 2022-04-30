using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopHandler : MonoBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float health;

    private static int _troopLayer = 6;

    private void Start()
    {
        StartMoving();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _troopLayer && (ghostEffect || !other.CompareTag(gameObject.tag)))
        {
            StopMoving();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartMoving();
    }

    public void StopMoving() => currentMovementSpeed = 0;
    public void StartMoving() => currentMovementSpeed = movementSpeed;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}

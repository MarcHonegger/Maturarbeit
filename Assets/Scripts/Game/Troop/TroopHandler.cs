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

    private const int k_TroopLayer = 6;

    private void Start()
    {
        StartMoving();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == k_TroopLayer && (ghostEffect || !other.CompareTag(gameObject.tag)))
        {
            StopMoving();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == k_TroopLayer)
        {
            StartMoving();
        }
    }

    public void StopMoving() => currentMovementSpeed = 0;
    public void StartMoving() => currentMovementSpeed = movementSpeed;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0.001)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

}

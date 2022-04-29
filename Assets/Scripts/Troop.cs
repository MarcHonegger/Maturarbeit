using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour
{
    public int energyCost;
    public float movementSpeed;
    public float currentMovementSpeed;
    public bool ghostEffect;
    public float health;

    private static int _troopLayer = 6;

    private void Start()
    {
        currentMovementSpeed = movementSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _troopLayer && (ghostEffect || !other.CompareTag(gameObject.tag)))
        {
            currentMovementSpeed = 0;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        currentMovementSpeed = movementSpeed;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
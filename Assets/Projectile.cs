using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float endPoint;
    public float shotSpeed;
    public float damage;
    private float _direction;

    private void Start()
    {
        _direction = PlayerManager.Instance.GetDirection(gameObject);
    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * (0.05f * _direction * shotSpeed);
    }

    private void Update()
    {
        if (transform.position.x * _direction >= endPoint * _direction)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && !other.CompareTag(gameObject.tag))
        {
            Hit(other.gameObject);
        }
    }

    private void Hit(GameObject enemy)
    {
        GameManager.Instance.troopManager.AttackTroop(new Attack(enemy.GetComponent<TroopHandler>(), damage));
        Destroy(gameObject);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRevenge : MonoBehaviour
{
    private GameObject _revengePrefab;

    public float distance;
    public float speed;

    private void Start()
    {
        _revengePrefab = gameObject;
        Invoke(nameof(Respawn), speed);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6)
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<TroopHandler>();
        enemy.TakeDamage(50);
    }

    private void Respawn()
    {
        var spawnManager = GameManager.instance.spawnManager;
        float leftSpawnPointX = spawnManager.spawnPointStart.x;
        float spawnPointDistanceX = spawnManager.spawnPointDistance.x;
        
        if (CompareTag("LeftPlayer"))
        {
            if(transform.position.x > leftSpawnPointX + spawnPointDistanceX)
                return;
        }
        else
        {
            if(transform.position.x < leftSpawnPointX)
                return;
        }
        
        _revengePrefab.transform.localScale = new Vector3(1, 1, 1);
        _revengePrefab.transform.Rotate(new Vector3(45, 0, 0));
        Instantiate(_revengePrefab, transform.position + (CompareTag("LeftPlayer") ? Vector3.right : Vector3.left) * distance, Quaternion.identity);
    }

    private void Update()
    {
        transform.localScale *= 0.995f;
    }
}

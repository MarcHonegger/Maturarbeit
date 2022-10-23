using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroRevenge : MonoBehaviour
{
    private GameObject _revengePrefab;
    private float _xScale;
    private float _yScale;

    public float distance;
    public float duration;

    private void Start()
    {
        _revengePrefab = gameObject;
        Invoke(nameof(Activate), duration);
        Destroy(gameObject, 2f);
        var localScale = transform.localScale;
        _xScale = localScale.x;
        _yScale = localScale.y;
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
        var leftSpawnPointX = spawnManager.spawnPointStart.x;
        var spawnPointDistanceX = spawnManager.spawnPointDistance.x;
        
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
        
        _revengePrefab.transform.localScale = new Vector3(_xScale, _yScale, 1);
        // _revengePrefab.transform.Rotate(new Vector3(45, 0, 0));
        Instantiate(_revengePrefab, transform.position + (CompareTag("LeftPlayer") ? Vector3.right : Vector3.left) * distance, Quaternion.identity);
    }

    private void Activate()
    {
    }

    private void Update()
    {
        transform.localScale *= 0.995f;
    }
}

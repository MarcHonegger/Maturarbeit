using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangePoint : MonoBehaviour
{
    private BoxCollider _collider;
    private List<GameObject> _enemiesInRange;
    public float ySize;
    public float zSize;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _enemiesInRange = new List<GameObject>();
    }

    public void UpdateRangeCollider(float range)
    {
        _collider.size = new Vector3(range, ySize, zSize);
        var offset = PlayerManager.Instance.GetDirection(transform.parent.gameObject) * range / 2;
        _collider.center = new Vector3(offset, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 || other.gameObject.CompareTag(gameObject.tag))
        {
            return;
        }

        var enemy = other.gameObject;
        _enemiesInRange.Add(enemy);
        if (_enemiesInRange.Count == 1)
        {
            EnemyInRange?.Invoke(enemy);
        }
    }


    public event Action<GameObject> EnemyInRange;
}
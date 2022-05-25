using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangePoint : MonoBehaviour
{
    private BoxCollider _collider;
    public readonly LinkedList<TroopHandler> enemiesInRange = new LinkedList<TroopHandler>();
    public float ySize;
    public float zSize;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    public void UpdateRangeCollider(float range)
    {
        _collider.size = new Vector3(range, ySize, zSize);
        var offset = PlayerManager.Instance.GetDirection(transform.parent.gameObject) * range / 2;
        _collider.center = new Vector3(offset, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 || other.gameObject.CompareTag(transform.parent.tag))
        {
            return;
        }

        var enemy = other.gameObject.GetComponent<TroopHandler>();
        enemiesInRange.AddLast(enemy);
        NewEnemyInRange?.Invoke(enemy);

        var enemyNode = enemiesInRange.Last;
        enemyNode.Value.Death += () =>
        {
            bool wasFirstEnemy = enemiesInRange.First == enemyNode;
            enemiesInRange.Remove(enemyNode);
            if (enemiesInRange.Count == 0)
                NoEnemyInRange?.Invoke();
            else if (wasFirstEnemy)
                NewEnemyInRange?.Invoke(enemiesInRange.First.Value);
        };
    }

    public event Action<TroopHandler> NewEnemyInRange;
    public event Action NoEnemyInRange;
}

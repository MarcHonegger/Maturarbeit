using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;

    private RangePoint _rangePoint;
    private TroopHandler _nextEnemy;
    private TroopHandler _troopHandler;

    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.EnemyInRange += OnNewEnemyInRange;
    }

    private void OnDestroy()
    {
        _rangePoint.EnemyInRange -= OnNewEnemyInRange;
        CancelInvoke();
    }

    private void Attack()
    {
        if (!_nextEnemy)
        {
            NoEnemyInRange();
        }
        GameManager.Instance.troopManager.AttackTroop(new Attack(_nextEnemy, attackDamage));
    }

    private void OnNewEnemyInRange(GameObject enemy)
    {
        _nextEnemy = enemy.GetComponent<TroopHandler>();
        _troopHandler.StopMoving();
        
        InvokeRepeating(nameof(Attack), 0, attackSpeed);
    }

    private void NoEnemyInRange()
    {
        CancelInvoke(nameof(Attack));
        _troopHandler.StartMoving();
    }
}
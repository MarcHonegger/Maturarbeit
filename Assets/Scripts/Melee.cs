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
    private Troop _nextEnemy;
    private Troop _troop;

    // Start is called before the first frame update
    void Start()
    {
        _troop = GetComponent<Troop>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.EnemyInRange += OnEnemyInRange;
    }

    private void OnDestroy()
    {
        _rangePoint.EnemyInRange -= OnEnemyInRange;
    }

    private void Attack()
    {
        if (!_nextEnemy)
        {
            NoEnemyInRange();
        }
        GameManager.Instance.troopManager.AttackTroop(new Attack(_nextEnemy, attackDamage));
    }

    private void OnEnemyInRange(GameObject enemy)
    {
        _nextEnemy = enemy.GetComponent<Troop>();
        _troop.StopMoving();
        
        InvokeRepeating(nameof(Attack), 0, attackSpeed);
    }

    private void NoEnemyInRange()
    {
        CancelInvoke(nameof(Attack));
        _troop.StartMoving();
    }
}
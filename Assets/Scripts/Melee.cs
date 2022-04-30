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
    private bool _attacking;

    // Start is called before the first frame update
    void Start()
    {
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.EnemyInRange += OnEnemyInRange;
    }

    private void OnDestroy()
    {
        _rangePoint.EnemyInRange -= OnEnemyInRange;
    }

    private void Update()
    {
        if (_nextEnemy)
        {
            if (_attacking) return;
            // Play Animation
            InvokeRepeating(nameof(Attack), 0, attackSpeed);
            _attacking = true;
        }
        else
        {
            NoEnemyInRange();
        }
    }
    
    private void Attack()
    {
        GameManager.Instance.troopManager.AttackTroop(new Attack(_nextEnemy, attackDamage));
    }

    private void OnEnemyInRange(GameObject enemy)
    {
        _nextEnemy = enemy.GetComponent<Troop>();
        GetComponent<Troop>().StopMoving();
    }

    private void NoEnemyInRange()
    {
        CancelInvoke();
        _attacking = false;
        GetComponent<Troop>().StartMoving();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Melee : MonoBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attackSpeed;
    public bool dieAfterAttack;

    private RangePoint _rangePoint;
    private TroopHandler _troopHandler;

    private Animator _animator;
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");
    private static readonly int StopAttackingAnimation = Animator.StringToHash("StopAttacking");

    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _animator = GetComponent<Animator>();

        _rangePoint.NewEnemyInRange += OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange += OnNoEnemyInRange;
    }

    private void OnDestroy()
    {
        _rangePoint.NewEnemyInRange -= OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange -= OnNoEnemyInRange;

        CancelInvoke();
    }

    private void Attack()
    {
        _troopHandler.StopMoving();
        GameManager.Instance.troopManager.AttackTroop(new Attack(_rangePoint.enemiesInRange.First.Value, attackDamage));
        _animator.SetTrigger(AttackAnimation);
    }

    private void OnNewEnemyInRange(TroopHandler enemy)
    {
        if (dieAfterAttack)
        {
            GameManager.Instance.troopManager.AttackTroop(new Attack(_rangePoint.enemiesInRange.First.Value, attackDamage));
            _troopHandler.Die();
            return;
        }
        _troopHandler.StopMoving();
        _animator.SetTrigger(AttackAnimation);
        InvokeRepeating(nameof(Attack), attackSpeed, attackSpeed);
    }

    private void OnNoEnemyInRange()
    {
        CancelInvoke(nameof(Attack));
        _animator.SetTrigger(StopAttackingAnimation);
        _troopHandler.StartMoving();
    }
}

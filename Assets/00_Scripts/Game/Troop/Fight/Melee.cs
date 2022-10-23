using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class Melee : NetworkBehaviour
{
    public float attackRange;
    public float attackDamage;
    public float attacksPerSecond;
    public float currentAttacksPerSecond;
    public bool dieAfterAttack;

    private RangePoint _rangePoint;
    private TroopHandler _troopHandler;

    private Animator _animator;
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");
    private static readonly int StopAttackingAnimation = Animator.StringToHash("StopAttacking");
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

    // Start is called before the first frame update
    
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _animator = GetComponent<Animator>();

        _rangePoint.NewEnemyInRange += OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange += OnNoEnemyInRange;

        currentAttacksPerSecond = attacksPerSecond;
        SetAttackSpeed();
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
        DealDamage();
        _animator.SetTrigger(AttackAnimation);
        
        Attacked?.Invoke();
    }

    public void BuffAttackSpeed(float attackSpeedBuff, bool absolute)
    {
        if(absolute)
            currentAttacksPerSecond = Mathf.Round((currentAttacksPerSecond + attackSpeedBuff) * 100) / 100;
        else
        {
            currentAttacksPerSecond = Mathf.Round((currentAttacksPerSecond * (1 + attackSpeedBuff) * 100)) / 100;
        }

        currentAttacksPerSecond = Mathf.Min(attacksPerSecond * 2.5f, currentAttacksPerSecond);
        
        SetAttackSpeed();
    }
    
    public void NerfAttackSpeed(float attackSpeedNerf, bool absolute)
    {
        if(absolute)
            currentAttacksPerSecond = Mathf.Round((currentAttacksPerSecond - attackSpeedNerf) * 100) / 100;
        else
        {
            currentAttacksPerSecond = Mathf.Round((currentAttacksPerSecond / (1 + attackSpeedNerf) * 100)) / 100;
        }

        currentAttacksPerSecond = Mathf.Max(attacksPerSecond * 0.75f, currentAttacksPerSecond);
        
        SetAttackSpeed();
    }
    
    public void SetAttackSpeed()
    {
        _animator.SetFloat(AttackSpeed, currentAttacksPerSecond);
        
        if(IsInvoking(nameof(Attack)))
        {
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 1 / currentAttacksPerSecond, 1 / currentAttacksPerSecond);
        }
    }

    public void SetAttackSpeed(float value)
    {
        _animator.SetFloat(AttackSpeed, currentAttacksPerSecond);
        
        if(IsInvoking(nameof(Attack)))
        {
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 1 / currentAttacksPerSecond, 1 / currentAttacksPerSecond);
        }
    }
    
    [Server]
    private void DealDamage()
    {
        GameManager.instance.troopManager.AttackTroop(new Attack(_rangePoint.enemiesInRange.First.Value, _troopHandler, attackDamage, AttackType.Melee));
    }

    private void OnNewEnemyInRange(TroopHandler enemy)
    {
        if (dieAfterAttack)
        {
            GameManager.instance.troopManager.AttackTroop(new Attack(enemy, _troopHandler, attackDamage, AttackType.Melee));
            _troopHandler.Die();
            return;
        }
        _troopHandler.StopMoving();
        _animator.SetTrigger(AttackAnimation);
        InvokeRepeating(nameof(Attack), 1 / attacksPerSecond, attacksPerSecond);
    }

    [ClientRpc]
    private void OnNoEnemyInRange()
    {
        CancelInvoke(nameof(Attack));
        _animator.SetTrigger(StopAttackingAnimation);
        _troopHandler.StartMoving();
    }
    
    public event Action Attacked;
}

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

public class Ranged : NetworkBehaviour
{
    public float attackRange;
    public float attacksPerSecond;
    public float currentAttacksPerSecond;
    public GameObject projectile;

    private int _direction;
    private RangePoint _rangePoint;
    public Transform shotPoint;
    private TroopHandler _nextEnemy;
    private TroopHandler _troopHandler;

    private Transform _projectiles;
    
    private Animator _animator;
    private static readonly int AttackAnimation = Animator.StringToHash("Attack");
    private static readonly int StopAttackingAnimation = Animator.StringToHash("StopAttacking");
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
    
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.NewEnemyInRange += OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange += OnNoEnemyInRange;

        _projectiles = GameObject.Find("projectiles").transform;

        _direction = PlayerManager.instance.GetDirection(gameObject);
        _animator = GetComponent<Animator>();

        if (_direction < 0)
        {
            var localPosition = shotPoint.localPosition;
            localPosition = new Vector3(localPosition.x * -1, localPosition.y, localPosition.z);
            shotPoint.localPosition = localPosition;
        }

        currentAttacksPerSecond = attacksPerSecond;
        SetAttackSpeed();
    }

    private void OnDestroy()
    {
        _rangePoint.NewEnemyInRange -= OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange -= OnNoEnemyInRange;
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

        if (!IsInvoking(nameof(Attack)))
            return;
        CancelInvoke(nameof(Attack));
        InvokeRepeating(nameof(Attack), 1f / currentAttacksPerSecond, 1f / currentAttacksPerSecond);
    }

    public void SetAttackSpeed(float value)
    {
        currentAttacksPerSecond = value;
        _animator.SetFloat(AttackSpeed, currentAttacksPerSecond);
        
        if(IsInvoking(nameof(Attack)))
        {
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 1 / currentAttacksPerSecond, 1 / currentAttacksPerSecond);
        }
    }

    private void Shoot()
    {
        var shot = Instantiate(projectile, shotPoint.position, quaternion.identity);
        shot.transform.tag = gameObject.tag;
        shot.transform.SetParent(_projectiles);
        shot.GetComponent<Projectile>().endPoint = shotPoint.position.x + (attackRange + 0.5f) * _direction;
        shot.GetComponent<Projectile>().shooter = GetComponent<TroopHandler>();
        // shot.transform.RotateAround(transform.position, Vector3.right, 45);
        
        Attacked?.Invoke();
    }
    
    private void OnNewEnemyInRange(TroopHandler enemy)
    {
        _troopHandler.StopMoving();
        _animator.SetTrigger(AttackAnimation);
        InvokeRepeating(nameof(Shoot), 1 / attacksPerSecond, 1 / attacksPerSecond);
    }

    [ClientRpc]
    private void OnNoEnemyInRange()
    {
        CancelInvoke(nameof(Shoot));
        _animator.SetTrigger(StopAttackingAnimation);
        _troopHandler.StartMoving();
    }
    
    public event Action Attacked;
}

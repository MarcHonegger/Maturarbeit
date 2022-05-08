using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public float attackRange;
    public float attackSpeed;
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
    
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.NewEnemyInRange += OnNewEnemyInRange;
        _rangePoint.NoEnemyInRange += OnNoEnemyInRange;

        _projectiles = GameObject.Find("projectiles").transform;

        _direction = PlayerManager.Instance.GetDirection(gameObject);
        _animator = GetComponent<Animator>();
    }

    private void Shoot()
    {
        var shot = Instantiate(projectile, shotPoint.position, quaternion.identity);
        shot.transform.tag = gameObject.tag;
        shot.transform.SetParent(_projectiles);
        shot.GetComponent<Projectile>().endPoint = shotPoint.position.x + (attackRange + 0.1f) * _direction;
        // shot.transform.RotateAround(shotPoint.position, Vector3.right, 45);
    }

    private void OnNewEnemyInRange(TroopHandler enemy)
    {
        _troopHandler.StopMoving();
        _animator.SetTrigger(AttackAnimation);
        InvokeRepeating(nameof(Shoot), attackSpeed, attackSpeed);
    }

    private void OnNoEnemyInRange()
    {
        CancelInvoke(nameof(Shoot));
        _animator.SetTrigger(StopAttackingAnimation);
        _troopHandler.StartMoving();
    }
}

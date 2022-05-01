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
    private TroopHandler _nextEnemy;
    private TroopHandler _troopHandler;

    private Transform _projectiles;
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rangePoint = GetComponentInChildren<RangePoint>();
        _rangePoint.UpdateRangeCollider(attackRange);
        _rangePoint.EnemyInRange += OnNewEnemyInRange;

        _projectiles = new GameObject("projectiles").transform;
        _projectiles.SetParent(transform);

        _direction = PlayerManager.Instance.GetDirection(gameObject);
    }

    private void Shoot()
    {
        if (!_nextEnemy)
        {
            NoEnemyInRange();
        }
        var shot = Instantiate(projectile, transform.position, quaternion.identity);
        shot.transform.tag = gameObject.tag;
        shot.transform.SetParent(_projectiles);
        shot.GetComponent<Projectile>().endPoint = transform.position.x + attackRange * _direction;
    }

    private void OnNewEnemyInRange(GameObject enemy)
    {
        _nextEnemy = enemy.GetComponent<TroopHandler>();
        _troopHandler.StopMoving();

        InvokeRepeating(nameof(Shoot), 0, attackSpeed);
    }

    private void NoEnemyInRange()
    {
        CancelInvoke(nameof(Shoot));
        _troopHandler.StartMoving();
    }
}

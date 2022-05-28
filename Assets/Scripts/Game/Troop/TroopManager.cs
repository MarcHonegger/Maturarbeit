using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;

public class Attack
{
    public Attack(TroopHandler enemy, TroopHandler troop, float damage, AttackType type)
    {
        target = enemy;
        attacker = troop;
        this.damage = damage;
        this.type = type;
    }
    public readonly TroopHandler target; 
    public readonly TroopHandler attacker; 
    public readonly float damage;
    public readonly AttackType type;
}

public class TroopManager : NetworkBehaviour
{
    private List<Attack> _attacks;
    private GameManager _gameManager;


    
    // TODO Start when Server Starts
    private void Start()
    {
        _attacks = new List<Attack>();
        InvokeRepeating(nameof(AttackPhase), 0, GameManager.Instance.tickRate);
    }

    [Server]
    public void AttackTroop(Attack attack)
    {
        _attacks.Add(attack);
    }

    [Server]
    private void AttackPhase()
    {
        if (_attacks.Count == 0)
        {
            return;
        }

        StringBuilder attackLog = new StringBuilder();
        foreach (Attack attack in _attacks)
        {
            if (!attack.target || attack.target.isDead)
            {
                continue;
            }

            Attack(attack.target, attack.attacker , attack.damage, attack.type);
            attackLog.Append($"| {attack.target.name} ({attack.damage})");
        }

        _attacks.Clear();

        Debug.Log($"AttackPhase: {attackLog}");
    }

    [Server]
    private void Attack(TroopHandler target, TroopHandler attacker, float damage, AttackType type)
    {
        target.TakeDamage(damage, attacker, type);
    }
}

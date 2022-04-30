using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public Attack(Troop enemy, float damage)
    {
        target = enemy;
        this.damage = damage;
    }
    public readonly Troop target; 
    public readonly float damage;
}

public class TroopManager : MonoBehaviour
{
    private List<Attack> _attacks;


    private void Start()
    {
        _attacks = new List<Attack>();
        InvokeRepeating(nameof(AttackPhase), 0, GameManager.Instance.tickRate);
    }

    public void AttackTroop(Attack attack)
    {
        _attacks.Add(attack);
    }

    private void AttackPhase()
    {
        var attackLog = "";
        foreach (var attack in _attacks)
        {
            attack.target.TakeDamage(attack.damage);
            attackLog += $"| {attack.target.name} ({attack.damage})";
        }

        _attacks = new List<Attack>();

        Debug.Log("AttackPhase: " + attackLog);
    }
}

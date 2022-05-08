using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Attack
{
    public Attack(TroopHandler enemy, float damage)
    {
        target = enemy;
        this.damage = damage;
    }
    public readonly TroopHandler target; 
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
            attack.target.TakeDamage(attack.damage);
            attackLog.Append($"| {attack.target.name} ({attack.damage})");
        }

        _attacks.Clear();

        Debug.Log($"AttackPhase: {attackLog}");
    }
}

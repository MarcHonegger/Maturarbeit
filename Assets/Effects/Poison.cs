using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoisonStats
{
    public PoisonStats(float tickRate, float damage, float duration)
    {
        this.tickRate = tickRate;
        this.damage = damage;
        this.duration = duration;
    }
    public readonly float tickRate;
    public readonly float damage;
    public readonly float duration;
}

public class Poison : MonoBehaviour
{
    private readonly PoisonStats _poisonStats = new PoisonStats(1, 1, 10);
    private TroopHandler _target;
    
    void Start()
    {
        _target = GetComponent<TroopHandler>();
        InvokeRepeating(nameof(PoisonDamage), _poisonStats.tickRate, _poisonStats.tickRate);
        Destroy(this, _poisonStats.duration);
        _target.healthBar.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Game/HealthBar/GreenFill");
    }

    private void OnDestroy()
    {
        _target.healthBar.ResetColor();
    }

    private void PoisonDamage()
    {
        _target.TakeDamage(_poisonStats.damage);
    }
}

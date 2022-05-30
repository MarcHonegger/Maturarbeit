using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float tickRate;
    public float damage;
    public float duration;
    private TroopHandler _target;
    
    void Start()
    {
        _target = GetComponent<TroopHandler>();
        InvokeRepeating(nameof(PoisonDamage), tickRate, tickRate);
        Destroy(this, duration);
    }

    private void PoisonDamage()
    {
        _target.TakeDamage(damage);
    }
}

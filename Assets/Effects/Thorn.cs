using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee = 0, Ranged = 1, Effect = 2
}

public class Thorn : MonoBehaviour
{
    private TroopHandler _troopHandler;

    public AttackType attackTypes;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _troopHandler.DamageTaken += OnDamageTaken;
    }

    // Update is called once per frame
    private void OnDamageTaken(TroopHandler attacker, AttackType type)
    {
        if(type == AttackType.Effect || !attacker)
            return;
        if(type == attackTypes)
            attacker.TakeDamage(damage);
    }
}

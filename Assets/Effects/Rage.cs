using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : MonoBehaviour
{
    public float rageBuff;
    private float _beforeBuff;
    
    void Start()
    {
        Melee melee = GetComponent<Melee>();
        if (melee)
        {
            _beforeBuff = melee.currentAttacksPerSecond;
            GetComponentInChildren<RangePoint>().NewEnemyInRange += (_) => melee.SetAttackSpeed(_beforeBuff);
            melee.Attacked += () =>
            {
                melee.BuffAttackSpeed(rageBuff, false);
            };
        }
        else
        {
            Ranged ranged = GetComponent<Ranged>();
            if (ranged)
            {
                // GetComponent<RangePoint>().NewEnemyInRange += () =>
                // ranged.Attacked += () => melee.BuffAttackSpeed(rageBuff, false);
            }
        }
    }
}

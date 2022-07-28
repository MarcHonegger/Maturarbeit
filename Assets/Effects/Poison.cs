using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Poison : MonoBehaviour
{
    public float tickRate;
    public float damage;
    public float duration;
    public int poisonId;
    private TroopHandler _target;
    
    void Start()
    {
        _target = GetComponentInParent<TroopHandler>();
        InvokeRepeating(nameof(PoisonDamage), 0, tickRate);
        Invoke(nameof(ResetHealthBarColor), duration);
        Invoke(nameof(SelfDestruct), duration);
        _target.healthBar.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Game/HealthBar/GreenFill");
    }

    public void RestartPoison()
    {
        CancelInvoke(nameof(ResetHealthBarColor));
        CancelInvoke(nameof(SelfDestruct));
        
        Invoke(nameof(SelfDestruct), duration);
        Invoke(nameof(ResetHealthBarColor), duration);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void ResetHealthBarColor()
    {
        _target.healthBar.ResetColor();
    }

    private void PoisonDamage()
    {
        GameManager.instance.troopManager.AttackTroop(new Attack(_target, null, damage, AttackType.Effect));
    }
}

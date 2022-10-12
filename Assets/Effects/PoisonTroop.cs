using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PoisonType
{
    OnDamageTaken = 1, OnDamageDone = 2
}

public class PoisonTroop : MonoBehaviour
{
    private TroopHandler _troopHandler;
    public GameObject poisonPrefab;
    public PoisonType poisonType;
    public int poisonId;
    
    public 
    
    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        if(poisonType == PoisonType.OnDamageTaken)
            _troopHandler.DamageTaken += OnDamageTaken;
        else if (poisonType == PoisonType.OnDamageDone)
            _troopHandler.DamageTaken += OnDamageTaken;
        poisonId = poisonPrefab.GetComponent<Poison>().poisonId;
    }

    // Update is called once per frame
    private void OnDamageTaken(TroopHandler attacker, AttackType type)
    {
        if(type == AttackType.Effect || !attacker || poisonType == PoisonType.OnDamageDone)
            return;
        Poison foundPoisons = attacker.GetComponentsInChildren<Poison>().FirstOrDefault(p => p.poisonId == poisonId);
        if (foundPoisons is null)
            Instantiate(poisonPrefab, attacker.transform, true);
        else
        {
            foundPoisons.RestartPoison();
        }
    }
}

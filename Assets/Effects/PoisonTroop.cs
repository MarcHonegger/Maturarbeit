using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTroop : MonoBehaviour
{
    private TroopHandler _troopHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _troopHandler.DamageTaken += OnDamageTaken;
    }

    // Update is called once per frame
    private void OnDamageTaken(TroopHandler attacker)
    {
        attacker.gameObject.AddComponent<Poison>();
    }
}

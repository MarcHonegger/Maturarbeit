using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revenge : MonoBehaviour
{
    private TroopHandler _troopHandler;

    public GameObject revengePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _troopHandler.Death += OnDeath;
    }

    void OnDeath()
    {
        GameObject revengeObject = Instantiate(revengePrefab, transform.position, Quaternion.identity);
        revengeObject.tag = gameObject.tag;
    }
}

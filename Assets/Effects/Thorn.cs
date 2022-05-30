using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    private TroopHandler _troopHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
    }
}

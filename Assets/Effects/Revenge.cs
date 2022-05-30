using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revenge : MonoBehaviour
{
    private TroopHandler _troopHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Melee : MonoBehaviour
{
    public int attackRange;
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<RangePoint>().UpdateRange(attackRange);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("test");
        Destroy(gameObject);
    }
}
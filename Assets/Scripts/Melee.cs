using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Melee : MonoBehaviour
{
    public int attackRange;
    public int attackSpeed;
    public int attackDamage;
    public Collider2D attackPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        attackPoint.offset = new Vector2(attackRange * 0.01f, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("test");
        Destroy(gameObject);
    }
}

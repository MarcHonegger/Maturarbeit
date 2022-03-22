using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovementTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.right * 0.05f;
    }
}

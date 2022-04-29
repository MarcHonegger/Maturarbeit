using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovementTest : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        var direction = gameObject.CompareTag("LeftPlayer") ? 1 : -1;
        transform.position += Vector3.right * 0.05f * GetComponent<Troop>().currentMovementSpeed * direction;
    }
}

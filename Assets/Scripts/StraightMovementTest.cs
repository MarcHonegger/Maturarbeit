using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovementTest : MonoBehaviour
{
    private Troop _troop;
    // Update is called once per frame
    private void Start()
    {
        _troop = GetComponent<Troop>();
    }
    void FixedUpdate()
    {
        var direction = PlayerManager.Instance.GetDirection(gameObject);
        transform.position += Vector3.right * (0.05f * _troop.currentMovementSpeed * direction);
    }
}

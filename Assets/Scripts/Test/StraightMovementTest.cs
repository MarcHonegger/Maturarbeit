using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovementTest : MonoBehaviour
{
    private TroopHandler _troopHandler;
    // Update is called once per frame
    private void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
    }
    void FixedUpdate()
    {
        var direction = PlayerManager.Instance.GetDirection(gameObject);
        transform.position += Vector3.right * (0.05f * _troopHandler.currentMovementSpeed * direction);
    }
}

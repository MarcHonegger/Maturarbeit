using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightMovementTest : MonoBehaviour
{
    private TroopHandler _troopHandler;
    private Rigidbody _rigidBody;
    // Update is called once per frame
    private void Start()
    {
        _troopHandler = GetComponent<TroopHandler>();
        _rigidBody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        int direction = PlayerManager.instance.GetDirection(gameObject);
        Vector3 targetPos = transform.position + Vector3.right * (_troopHandler.currentMovementSpeed * direction);
        
        // Check if a mate is in front of this troop
        int bitMask = 1 << LayerMask.GetMask("Troop");
        Ray ray = new Ray(targetPos, Vector3.right * direction);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Abs(targetPos.x - transform.position.x) + GetComponent<BoxCollider>().size.x, bitMask))
            if(hit.transform.CompareTag(gameObject.tag))
                return;
        
        _rigidBody.MovePosition(targetPos);
    }
}

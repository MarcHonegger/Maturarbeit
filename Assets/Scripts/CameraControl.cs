using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private float _speedInput = 0f;
    public float cameraSpeed;
    
    void FixedUpdate()
    {
        var cameraTransform = transform;
        var cameraPosition = cameraTransform.position;
        var targetPosition = cameraPosition + new Vector3(_speedInput * cameraSpeed * 4f, 0f, 0f); 
        //limiting camera movement left side
        if (cameraPosition.x <= -5f && _speedInput == -1f)
        {
            UnityEngine.Debug.Log($"Limited Camera Left Side");
            return;
        }
        transform.position = Vector3.Lerp(cameraPosition, targetPosition, 0.1f);
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"Speed is set to {context.ReadValue<float>()}");
        _speedInput = context.ReadValue<float>();
    }
}

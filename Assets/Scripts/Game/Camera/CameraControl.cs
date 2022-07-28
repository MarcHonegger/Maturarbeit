using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    private float _speedInput = 0f;
    private Camera _cam;
    public float cameraSpeed;
    public float leftEnd;
    public float rightEnd;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        var cameraTransform = transform;
        var cameraPosition = cameraTransform.position;
        var targetPosition = cameraPosition + new Vector3(_speedInput * cameraSpeed * 4f, 0f, 0f);
        if (cameraPosition.x <= leftEnd && _speedInput < 0 || cameraPosition.x >= rightEnd && _speedInput > 0)
        {
            return;
        }

        transform.position = Vector3.Lerp(cameraPosition, targetPosition, 0.1f);
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        // Debug.Log($"Speed is set to {context.ReadValue<float>()}");
        _speedInput = context.ReadValue<float>();
    }
    
    public void ChangeZoom(InputAction.CallbackContext context)
    {
        // Debug.Log($"Speed is set to {context.ReadValue<float>()}");
        _cam.fieldOfView = 5;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControll : MonoBehaviour
{
    private float _speedInput = 0f;

    public float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPosition = transform.position + new Vector3(_speedInput * cameraSpeed, 0f, 0f);
        transform.position = targetPosition;
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"Speed is set to {context.ReadValue<float>()}");
        _speedInput = context.ReadValue<float>();
    }
}

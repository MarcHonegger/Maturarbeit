using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControll : MonoBehaviour
{
    private float speedInput = 0f;

    public float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(speedInput * cameraSpeed, 0, 0);
    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        UnityEngine.Debug.Log($"Speed is set to {context.ReadValue<float>()}");
        speedInput = context.ReadValue<float>();

    }
}

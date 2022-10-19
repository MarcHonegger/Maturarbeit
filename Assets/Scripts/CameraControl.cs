using System;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : NetworkBehaviour
{
    private float _speedInput = 0f;
    private float _zoomInput = 0f;
    private Camera _cam;
    public int minFieldOfView;
    public int maximumFieldOfView;
    public float cameraSpeed;
    private int leftEnd = 0;
    private int rightEnd;

    private void Start()
    {
        _cam = GetComponent<Camera>();

        rightEnd = (int) GameManager.instance.spawnManager.spawnPointDistance.x;
        setClientPos();
        
    }

    void setClientPos()
    {
        if (isClientOnly)
        {
            transform.position = new Vector3(100, 37.5f, -30f);
        }
    }
    
    
    private GameObject getObject()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "Main Camera")
            {
                return go;
            }
        }
        return null;
    }

    void FixedUpdate()
    {
        var fieldOfView = _cam.fieldOfView;
        fieldOfView -= _zoomInput;
        _cam.fieldOfView = fieldOfView;
        _cam.fieldOfView = Mathf.Min(maximumFieldOfView, Math.Max(minFieldOfView, fieldOfView));
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
        _zoomInput = context.ReadValue<float>();
        onZoomChange.Invoke( CalculatedZoom());
    }

    public float CalculatedZoom()
    {
        return _cam.fieldOfView / maximumFieldOfView;
    }
    
    public Action<float> onZoomChange;
}
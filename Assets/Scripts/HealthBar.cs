using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private CameraControl _cameraControl;
    
    private Sprite _redHealthBarFill;
    private Sprite _blueHealthBarFill;

    private void Start()
    {
        _cameraControl = FindObjectOfType<CameraControl>();
        _cameraControl.onZoomChange += FitToZoom;
        FitToZoom(_cameraControl.CalculatedZoom());
        
        _redHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/RedFill");
        _blueHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/BlueFill");
        ResetColor();
    }

    private void OnDestroy()
    {
        _cameraControl.onZoomChange -= FitToZoom;
    }
    
    public void ResetColor()
    {
        transform.GetChild(1).GetComponent<Image>().sprite = CompareTag("LeftPlayer") ? _redHealthBarFill : _blueHealthBarFill;
    }

    public void SetMaximumHealth(float maximum)
    {
        slider.maxValue = maximum;
    }

    public void ChangeHealth(float value)
    {
        slider.value += value;
    }

    private void FitToZoom(float zoom)
    {
        transform.localScale = (1f - zoom / 3) * Vector3.one;
    }
    
}

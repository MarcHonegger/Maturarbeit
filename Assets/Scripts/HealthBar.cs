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

    private void Start()
    {
        _cameraControl = FindObjectOfType<CameraControl>();
        _cameraControl.onZoomChange += FitToZoom;
        FitToZoom(_cameraControl.CalculatedZoom());
    }

    private void OnDestroy()
    {
        FindObjectOfType<CameraControl>().onZoomChange -= FitToZoom;
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

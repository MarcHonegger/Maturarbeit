using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    private Sprite _redHealthBarFill;
    private Sprite _blueHealthBarFill;

    private void Start()
    {
        _redHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/RedFill");
        _blueHealthBarFill = Resources.Load<Sprite>("Game/HealthBar/BlueFill");
        ResetColor();
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
    
}

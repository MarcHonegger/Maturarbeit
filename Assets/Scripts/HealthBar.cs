using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaximumHealth(float maximum)
    {
        slider.maxValue = maximum;
    }

    public void ChangeHealth(float value)
    {
        slider.value += value;
    }
}

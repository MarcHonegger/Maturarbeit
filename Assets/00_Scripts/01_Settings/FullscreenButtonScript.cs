using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenButtonScript : MonoBehaviour
{
    public Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        SettingsManager.instance.ResetSettings += Reset;
        Reset();
    }

    private void OnDestroy()
    {
        SettingsManager.instance.ResetSettings -= Reset;
    }

    void Reset()
    {
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") != 0;
        }
    }
}

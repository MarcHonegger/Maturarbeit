using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Manager;
using TMPro;
using UnityEngine;

public class ResolutionHasChangedManager : MonoBehaviour
{
    private float _countDown;
    public TextMeshProUGUI countDownText;

    private void Start()
    {
        _countDown = 5;
    }

    private void Update()
    {
        _countDown -= Time.deltaTime;
        countDownText.text = Mathf.Ceil(_countDown).ToString(CultureInfo.InvariantCulture);
        if (_countDown <= 0)
        {
            CancelButtonPressed();
        }
    }

    public void CancelButtonPressed()
    {
        SettingsManager.instance.currentResolution = PlayerPrefs.GetInt("currentResolution");
        SettingsManager.instance.isFullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
        SettingsManager.instance.SetVideoSettingsUI();
        SettingsManager.instance.SetResolution();
        
        SettingsManager.instance.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
    
    public void AcceptButtonPressed()
    {
        PlayerPrefs.SetInt("currentResolution", SettingsManager.instance.currentResolution);
        PlayerPrefs.SetInt("fullscreen", SettingsManager.instance.isFullscreen ? 1 : 0);
        SettingsManager.instance.SetVideoSettingsUI();
        
        SettingsManager.instance.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
}

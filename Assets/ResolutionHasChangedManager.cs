using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class ResolutionHasChangedManager : MonoBehaviour
{
    public void CancelButtonPressed()
    {
        SettingsManager.instance.currentResolution = PlayerPrefs.GetInt("currentResolution");
        SettingsManager.instance.SetResolutionText();
        SettingsManager.instance.SetResolution();
        
        SettingsManager.instance.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
    
    public void AcceptButtonPressed()
    {
        PlayerPrefs.SetInt("currentResolution", SettingsManager.instance.currentResolution);
        SettingsManager.instance.SetResolutionText();
        
        SettingsManager.instance.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
}

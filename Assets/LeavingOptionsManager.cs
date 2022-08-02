using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class LeavingOptionsManager : MonoBehaviour
{
    private SettingsManager _settingsManager;
    // Start is called before the first frame update
    void Start()
    {
        _settingsManager = FindObjectOfType<SettingsManager>();
    }

    public void CancelButtonPressed()
    {
        _settingsManager.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
    
    public void AcceptButtonPressed()
    {
        _settingsManager.LoadValues();
        _settingsManager.LoadStartPage();
    }
}

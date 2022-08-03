using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class LeavingOptionsManager : MonoBehaviour
{
    public void CancelButtonPressed()
    {
        SettingsManager.instance.optionInteractable.SetActive(true);
        Destroy(gameObject);
    }
    
    public void AcceptButtonPressed()
    {
        SettingsManager.instance.LoadValues();
        SettingsManager.instance.LoadStartPage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultVolumeScript : MonoBehaviour
{
    public void Start()
    {
        var currentVolume = PlayerPrefs.GetFloat("volume");
        UnityEngine.Debug.Log($"Starting with volume: {currentVolume}");
    }

    

    
}

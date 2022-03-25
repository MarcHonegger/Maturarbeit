using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    private Slider _volumeSlider;
    
    public void Start()
    {
        _volumeSlider = GetComponent<Slider>();
        _volumeSlider.onValueChanged.AddListener(OnVolumeChange);
        var currentVolume = PlayerPrefs.GetFloat("Volume");
        PlayerPrefs.SetFloat("volume", currentVolume);
        _volumeSlider.value = currentVolume;
    }

    public void OnDestroy()
    {
        _volumeSlider.onValueChanged.RemoveListener(OnVolumeChange);
    }

    private static void OnVolumeChange(float value)
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        PlayerPrefs.SetFloat("volume", value);
    }
}

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
        var currentVolume = PlayerPrefs.GetFloat("volume");
        _volumeSlider.value = currentVolume;
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
        Debug.Log($"you final volume: {AudioListener.volume}");
        //causes Error
        //_volumeSlider.onValueChanged.RemoveListener(OnVolumeChange);
    }

    private static void OnVolumeChange(float value)
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        Debug.Log($"{AudioListener.volume}");
        PlayerPrefs.SetFloat("volume", value);
    }
}

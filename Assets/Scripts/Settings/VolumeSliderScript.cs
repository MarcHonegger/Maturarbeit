using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    private Slider _volumeSlider;
    public Sprite darkHandler;
    public Sprite brightHandler;
    public Image handlerImage;
    public Sprite darkFill;
    public Sprite brightFill;
    public Image fillImage;
    
    public void Start()
    {
        SettingsManager.instance.ResetSettings += Reset;
        Reset();
    }

    public void ChangeColor(bool muted)
    {
        if (muted)
        {
            fillImage.sprite = darkFill;
            handlerImage.sprite = darkHandler;
        } else
        {
            fillImage.sprite = brightFill;
            handlerImage.sprite = brightHandler;
        }
    }

    private void OnDestroy()
    {
        SettingsManager.instance.ResetSettings -= Reset;
    }

    private void Reset()
    {
        _volumeSlider = GetComponent<Slider>();
        var currentVolume =  PlayerPrefs.GetFloat("volume");
        _volumeSlider.value = currentVolume;
        
        ChangeColor(PlayerPrefs.GetInt("muted") != 0);
    }

    /*
    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
        Debug.Log($"you final volume: {AudioListener.volume}");
        //causes Error
        //_volumeSlider.onValueChanged.RemoveListener(OnVolumeChange);
    }

    private static void OnVolumeChange(float volume)
    {
        PlayerPrefs.SetFloat("volume", value);
        AudioListener.volume = volume;
        Debug.Log($"{AudioListener.volume}");
    }
    */
}

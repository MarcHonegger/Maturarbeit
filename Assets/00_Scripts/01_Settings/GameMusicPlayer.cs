using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMusicPlayer : MonoBehaviour
{
    public static GameMusicPlayer instance;
    private AudioSource _audioSource;
    
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("volume"))
        {
            SetVolume(1f);
            PlayerPrefs.SetFloat("volume", 1f);
        }
        if(!PlayerPrefs.HasKey("muted"))
        {
            Mute(false);
            PlayerPrefs.SetInt("muted", 0);
        }
        SetVolume(PlayerPrefs.GetFloat("volume"));
        Mute(PlayerPrefs.GetInt("muted") == 1);

        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this; 
        DontDestroyOnLoad(gameObject);
    }
    
    public void SetVolume(float volume)
    {
        if(volume <= 0.01f)
            Mute(true);
        _audioSource.volume = volume;
    }
    
    public void Mute(bool muted)
    {
        _audioSource.mute = muted;
    }
}

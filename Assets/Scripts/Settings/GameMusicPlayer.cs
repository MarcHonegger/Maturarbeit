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
        if(!PlayerPrefs.HasKey("volume"))
            PlayerPrefs.SetFloat("Volume", 1f);
        SetVolume(PlayerPrefs.GetFloat("volume"));

        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this; 
        DontDestroyOnLoad(gameObject);
    }
    
    public void SetVolume(float volume)
    {
        _audioSource.volume = volume;
    }
    
    public void Mute(bool muted)
    {
        _audioSource.mute = muted;
    }
}

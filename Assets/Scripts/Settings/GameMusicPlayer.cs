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
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this; 
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("volume");
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

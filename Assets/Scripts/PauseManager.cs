using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using networking;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public VolumeSliderScript volumeSliderScript;
    public Toggle muteToggle;
    private bool _isMuted;
    public List<ParticleSystem> snowParticleSystems;
    public NewPlayerManager newPlayerManager;
    public Slider snowSlider;
    public Image snowSliderHandlerImage;
    public Sprite darkHandler;
    public Sprite brightHandler;
    public Slider volumeSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("snowAmount"))
        {
            var savedAmount = PlayerPrefs.GetFloat("snowAmount");
            snowSlider.value = savedAmount;
            SetSnowAmount(savedAmount);
        }
        else
        {
            snowSlider.value = 1f;
            SetSnowAmount(1f);
            PlayerPrefs.SetFloat("snowAmount", 1f);
        }
        ChangeSnowSliderColor(PlayerPrefs.GetFloat("snowAmount") <= 0.01f);

        if(PlayerPrefs.HasKey("volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        if(PlayerPrefs.HasKey("muted"))
        {
            _isMuted = PlayerPrefs.GetInt("muted") == 1;
            volumeSliderScript.ChangeColor(_isMuted);
        }
        gameObject.SetActive(false);
    }

    public void SetMute(bool muted)
    {
        GameMusicPlayer.instance.Mute(muted);
        _isMuted = muted;
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        // remove Mute
        if (_isMuted)
        {
            GameMusicPlayer.instance.Mute(false);
            _isMuted = false;
            muteToggle.isOn = false;
            volumeSliderScript.ChangeColor(false);
        }
        if (volume <= 0.01f)
        {
            GameMusicPlayer.instance.Mute(true);
            _isMuted = true;
            muteToggle.isOn = true;
            volumeSliderScript.ChangeColor(true);
        }
        
        GameMusicPlayer.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetSnowAmount(float snowAmount)
    {
        ChangeSnowSliderColor(snowAmount <= 0.01f);
        foreach (var ps in snowParticleSystems)
        {
            var main = ps.main;
            main.maxParticles = (int) (1000 * snowAmount);
        }
        PlayerPrefs.SetFloat("snowAmount", snowAmount);
    }

    private void ChangeSnowSliderColor(bool turnedOff)
    {
        snowSliderHandlerImage.sprite = turnedOff ? darkHandler : brightHandler;
    }

    public void LoadStartScene()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        newPlayerManager.RpcAbortGame();
    }
}

using System.Collections;
using System.Collections.Generic;
using Mirror;
using networking;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public VolumeSliderScript volumeSlider;
    public Toggle muteToggle;
    private bool _isMuted;
    public List<ParticleSystem> snowParticleSystems;
    public NewPlayerManager newPlayerManager; 

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
            volumeSlider.ChangeColor(false);
        }

        GameMusicPlayer.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetSnowAmount(float snowAmount)
    {
        foreach (var ps in snowParticleSystems)
        {
            var main = ps.main;
            main.maxParticles = (int) (1000 * snowAmount);
        }
    }

    public void LoadStartScene()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        newPlayerManager.RpcAbortGame();
    }
}

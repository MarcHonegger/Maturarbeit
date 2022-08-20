using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggleScript : MonoBehaviour
{
    public Toggle muteToggle;
    public VolumeSliderScript volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(SettingsManager.instance)
            SettingsManager.instance.ResetSettings += Reset;
        Reset();
    }

    private void OnDestroy()
    {
        if(SettingsManager.instance)
            SettingsManager.instance.ResetSettings -= Reset;
    }

    void Reset()
    {
        var wasMuted = false;
        if (PlayerPrefs.HasKey("muted"))
        { 
            wasMuted = PlayerPrefs.GetInt("muted") != 0;
        }
        GameMusicPlayer.instance.Mute(wasMuted);
        muteToggle.isOn = wasMuted;
    }
}

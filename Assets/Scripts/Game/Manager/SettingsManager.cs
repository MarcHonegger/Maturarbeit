using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Button homeButton;
    public Image homeButtonImage;
    public TextMeshProUGUI resolutionText;
    public List<Resolution> resolutions = new List<Resolution>();
    private float _currentVolume;
    private bool _unsavedChanges;
    private bool _isFullscreen;
    private int _currentResolution;

    private void Start()
    {
        if(PlayerPrefs.HasKey("settings"))
            LoadValues();
        else
        {
            Safe();
        }
    }

    private void LoadValues()
    {
        SetVolume(PlayerPrefs.GetFloat("volume"));
        _currentResolution = PlayerPrefs.GetInt("currentResolution");
        _isFullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
        SetResolutionText();
        _unsavedChanges = false;
        DeactivateSafeButton();
    }

    public void LoadStartPage()
    {
        SceneManager.LoadScene("StartPageScene");
    }

    public void SetFullscreen(bool fullscreen)
    {
        _isFullscreen = fullscreen;
        ActivateSafeButton();
    }

    public void SetVolume(float volume)
    {
        _currentVolume = volume;
        GameMusicPlayer.instance.SetVolume(_currentVolume);
        ActivateSafeButton();
    }

    private void SetResolution()
    {
        var currentResolution = resolutions[_currentResolution];
        Screen.SetResolution(currentResolution.horizontal, currentResolution.vertical, _isFullscreen);
        ActivateSafeButton();
    }

    private void SetResolutionText()
    {
        var currentResolution = resolutions[_currentResolution];
        resolutionText.SetText($"{currentResolution.horizontal} x {currentResolution.vertical}");
    }

    public void ResolutionUp()
    {
        _currentResolution++;
        if (_currentResolution > resolutions.Count - 1)
        {
            _currentResolution = 0;
        }
        SetResolutionText();
        ActivateSafeButton();
    }
    
    public void ResolutionDown()
    {
        _currentResolution--;
        if (_currentResolution < 0)
        {
            _currentResolution = resolutions.Count - 1;
        }
        SetResolutionText();
        ActivateSafeButton();
    }

    private void ActivateSafeButton()
    {
        if(_unsavedChanges)
            return;
        _unsavedChanges = true;
        homeButton.enabled = true;
        homeButtonImage.color = Color.white;
    }

    private void DeactivateSafeButton()
    {
        homeButton.enabled = false;
        homeButtonImage.color = new Color(200, 200, 200, 0.4f);
    }

    public void Safe()
    {
        SetResolution();
        
        _unsavedChanges = false;
        DeactivateSafeButton();
        PlayerPrefs.SetFloat("volume", _currentVolume);
        PlayerPrefs.SetInt("fullscreen", _isFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("currentResolution", _currentResolution);
        PlayerPrefs.SetInt("settings", 0);
    }
}

[Serializable]
public class Resolution {
    public int horizontal, vertical;
}

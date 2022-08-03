using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Manager
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager instance;
        public Button saveButton;
        public Image saveButtonImage;
        public Button cancelButton;
        public Image cancelButtonImage;
        public List<TextMeshProUGUI> resolutionTexts;
        public List<Resolution> resolutions;
        public GameObject leavingOptionsPrefab;
        public Transform canvasTransform;
        public GameObject optionInteractable;
        private float _currentVolume;
        private bool _unsavedChanges;
        private bool _isFullscreen;
        private bool _isMuted;
        private int _currentResolution;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            if(PlayerPrefs.HasKey("settings"))
                LoadValues();
            else
            {
                Save();
            }
        }

        public void LoadStartPage()
        {
            if(_unsavedChanges)
                OpenLeavingOptions();
            else
            {
                SceneManager.LoadScene("StartPageScene");
            }
        }

        private void OpenLeavingOptions()
        {
            optionInteractable.SetActive(false);
            Instantiate(leavingOptionsPrefab, canvasTransform);
        }

        public void SetFullscreen(bool fullscreen)
        {
            _isFullscreen = fullscreen;
            ActivateSaveButton();
        }
        
        public void SetMute(bool muted)
        {
            GameMusicPlayer.instance.Mute(muted);
            _isMuted = muted;
            ActivateSaveButton();
        }

        public void SetVolume(float volume)
        {
            _currentVolume = volume;
            GameMusicPlayer.instance.SetVolume(_currentVolume);
            ActivateSaveButton();
        }

        private void SetResolution()
        {
            var currentResolution = resolutions[_currentResolution];
            Screen.SetResolution(currentResolution.horizontal, currentResolution.vertical, _isFullscreen);
            SetResolutionText();
            ActivateSaveButton();
        }

        private void SetResolutionText()
        {
            var currentResolution = resolutions[_currentResolution];
            resolutionTexts[0].SetText($"{currentResolution.horizontal}");
            resolutionTexts[1].SetText($"{currentResolution.vertical}");
        }

        public void ResolutionUp()
        {
            _currentResolution++;
            if (_currentResolution > resolutions.Count - 1)
            {
                _currentResolution = 0;
            }
            SetResolutionText();
            ActivateSaveButton();
        }
    
        public void ResolutionDown()
        {
            _currentResolution--;
            if (_currentResolution < 0)
            {
                _currentResolution = resolutions.Count - 1;
            }
            SetResolutionText();
            ActivateSaveButton();
        }

        private void ActivateSaveButton()
        {
            if(_unsavedChanges)
                return;
            _unsavedChanges = true;
            saveButton.enabled = true;
            saveButtonImage.color = Color.white;
            cancelButton.enabled = true;
            cancelButtonImage.color = Color.white;
        }

        private void DeactivateSaveButton()
        {
            saveButton.enabled = false;
            saveButtonImage.color = new Color(200, 200, 200, 0.4f);
            cancelButton.enabled = false;
            cancelButtonImage.color = new Color(200, 200, 200, 0.4f);
        }

        public void LoadValues()
        {
            _currentVolume = PlayerPrefs.GetFloat("volume");
            _currentResolution = PlayerPrefs.GetInt("currentResolution");
            _isFullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
            _isMuted = PlayerPrefs.GetInt("muted") != 0;
            
            SetVolume(_currentVolume);
            SetResolution();
            ResetSettings?.Invoke();
            
            _unsavedChanges = false;
            DeactivateSaveButton();
        }
        
        public event Action ResetSettings;

        public void Save()
        {
            SetResolution();   
            _unsavedChanges = false;
            DeactivateSaveButton();
            PlayerPrefs.SetFloat("volume", _currentVolume);
            PlayerPrefs.SetInt("fullscreen", _isFullscreen ? 1 : 0);
            PlayerPrefs.SetInt("muted", _isMuted ? 1 : 0);
            PlayerPrefs.SetInt("currentResolution", _currentResolution);
            PlayerPrefs.SetInt("settings", 0);
        }
    }

    [Serializable]
    public class Resolution {
        public int horizontal, vertical;
    }
}
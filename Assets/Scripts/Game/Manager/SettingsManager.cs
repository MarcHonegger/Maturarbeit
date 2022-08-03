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
        public GameObject videoSettingsHaveChangedPrefab;
        public Transform canvasTransform;
        public GameObject optionInteractable;
        public int currentResolution;
        private float _currentVolume;
        private bool _unsavedChanges;
        private bool _unsavedVideoChanges;
        private bool _isFullscreen;
        private bool _isMuted;

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
        
        private void OpenVideoSettingsHaveChanged()
        {
            optionInteractable.SetActive(false);
            Instantiate(videoSettingsHaveChangedPrefab, canvasTransform);
        }

        public void SetFullscreen(bool fullscreen)
        {
            _isFullscreen = fullscreen;
            _unsavedVideoChanges = true;
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

        public void SetResolution()
        {
            var currentResolution = resolutions[this.currentResolution];
            Screen.SetResolution(currentResolution.horizontal, currentResolution.vertical, _isFullscreen);
            SetResolutionText();
            _unsavedVideoChanges = false;
        }

        public void SetResolutionText()
        {
            var resolution = resolutions[this.currentResolution];
            resolutionTexts[0].SetText($"{resolution.horizontal}");
            resolutionTexts[1].SetText($"{resolution.vertical}");
        }

        public void ResolutionUp()
        {
            currentResolution++;
            if (currentResolution > resolutions.Count - 1)
            {
                currentResolution = 0;
            }
            SetResolutionText();
            _unsavedVideoChanges = true;
            ActivateSaveButton();
        }
    
        public void ResolutionDown()
        {
            currentResolution--;
            if (currentResolution < 0)
            {
                currentResolution = resolutions.Count - 1;
            }
            SetResolutionText();
            _unsavedVideoChanges = true;
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
            currentResolution = PlayerPrefs.GetInt("currentResolution");
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
            if (_unsavedVideoChanges)
            {
                SetResolution();   
                OpenVideoSettingsHaveChanged();
            }
            _unsavedChanges = false;
            DeactivateSaveButton();
            PlayerPrefs.SetFloat("volume", _currentVolume);
            PlayerPrefs.SetInt("fullscreen", _isFullscreen ? 1 : 0);
            PlayerPrefs.SetInt("muted", _isMuted ? 1 : 0);
            PlayerPrefs.SetInt("settings", 0);
        }

        private bool ChangeCheck()
        {
            return true;
        }
    }

    [Serializable]
    public class Resolution {
        public int horizontal, vertical;
    }
}
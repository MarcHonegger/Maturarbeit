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
        public Toggle fullscreenToggle;
        public GameObject leavingOptionsPrefab;
        public GameObject videoSettingsHaveChangedPrefab;
        public Transform canvasTransform;
        public GameObject optionInteractable;
        public int currentResolution;
        public bool isFullscreen;
        private float _currentVolume;
        private bool _unsavedVideoChanges;
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
                GenerateSettingsPlayerPrefs();
            }
        }

        public void LoadStartPage()
        {
            if(SettingsHaveChanged())
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
            isFullscreen = fullscreen;
            _unsavedVideoChanges = true;
            ChangeCheck();
        }
        
        public void SetMute(bool muted)
        {
            GameMusicPlayer.instance.Mute(muted);
            _isMuted = muted;
            ChangeCheck();
        }

        public void SetVolume(float volume)
        {
            _currentVolume = volume;
            GameMusicPlayer.instance.SetVolume(_currentVolume);
            ChangeCheck();
        }

        public void SetResolution()
        {
            var resolution = resolutions[currentResolution];
            Screen.SetResolution(resolution.horizontal, resolution.vertical, isFullscreen);
            SetVideoSettingsUI();
            _unsavedVideoChanges = false;
        }

        public void SetVideoSettingsUI()
        {
            var resolution = resolutions[currentResolution];
            resolutionTexts[0].SetText($"{resolution.horizontal}");
            resolutionTexts[1].SetText($"{resolution.vertical}");
            fullscreenToggle.isOn = isFullscreen;
        }

        public void ResolutionUp()
        {
            currentResolution++;
            if (currentResolution > resolutions.Count - 1)
            {
                currentResolution = 0;
            }
            SetVideoSettingsUI();
            _unsavedVideoChanges = true;
            ChangeCheck();
        }
    
        public void ResolutionDown()
        {
            currentResolution--;
            if (currentResolution < 0)
            {
                currentResolution = resolutions.Count - 1;
            }
            SetVideoSettingsUI();
            _unsavedVideoChanges = true;
            ChangeCheck();
        }

        private void ActivateSaveButtons()
        {
            saveButton.enabled = true;
            saveButtonImage.color = Color.white;
            cancelButton.enabled = true;
            cancelButtonImage.color = Color.white;
        }

        private void DeactivateSaveButtons()
        {
            saveButton.enabled = false;
            saveButtonImage.color = new Color(200, 200, 200, 0.4f);
            cancelButton.enabled = false;
            cancelButtonImage.color = new Color(200, 200, 200, 0.4f);
        }

        private void GenerateSettingsPlayerPrefs()
        {
            PlayerPrefs.SetInt("settings", 0);
            PlayerPrefs.SetFloat("volume", _currentVolume);
            PlayerPrefs.SetInt("muted", _isMuted ? 1 : 0);
            PlayerPrefs.SetInt("currentResolution", 0);
            PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
        }

        public void LoadValues()
        {
            _currentVolume = PlayerPrefs.GetFloat("volume");
            currentResolution = PlayerPrefs.GetInt("currentResolution");
            isFullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
            _isMuted = PlayerPrefs.GetInt("muted") != 0;
            
            SetVolume(_currentVolume);
            SetResolution();
            ResetSettings?.Invoke();
            
            ChangeCheck();
        }
        
        public event Action ResetSettings;

        public void Save()
        {
            if (_unsavedVideoChanges)
            {
                SetResolution();   
                OpenVideoSettingsHaveChanged();
            }
            PlayerPrefs.SetFloat("volume", _currentVolume);
            PlayerPrefs.SetInt("muted", _isMuted ? 1 : 0);
            PlayerPrefs.SetInt("settings", 0);
            
            ChangeCheck();
        }

        private void ChangeCheck()
        {
            if (SettingsHaveChanged())
            {
                ActivateSaveButtons();
            }
            else
            {
                DeactivateSaveButtons();
            }
        }

        private bool SettingsHaveChanged()
        {
            return
                // Audio settings
                !Mathf.Approximately(PlayerPrefs.GetFloat("volume"), _currentVolume) ||
                PlayerPrefs.GetInt("muted").AsBool() != _isMuted ||
                // Video settings
                PlayerPrefs.GetInt("fullscreen").AsBool() != isFullscreen ||
                currentResolution != PlayerPrefs.GetInt("currentResolution");
        }
    }

    [Serializable]
    public class Resolution {
        public int horizontal, vertical;
    }

    internal static class PlayerPrefExtensions
    {
        internal static bool AsBool(this int playerPrefsInt) => playerPrefsInt != 0;
    }
}
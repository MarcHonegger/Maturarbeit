using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenButtonScript : MonoBehaviour
{
    private Toggle _fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        _fullscreenToggle = GetComponent<Toggle>();
        _fullscreenToggle.onValueChanged.AddListener(OnToggleFullscreen);
        _fullscreenToggle.isOn = PlayerPrefs.GetString("FullscreenOn") == "Yes";

    }

    public void OnDestroy()
    {
        _fullscreenToggle.onValueChanged.RemoveListener(OnToggleFullscreen);
    }

    private static void OnToggleFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetString("FullscreenOn", value ? "Yes" : "No");
    }
}

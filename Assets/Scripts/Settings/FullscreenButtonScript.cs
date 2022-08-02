using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenButtonScript : MonoBehaviour
{
    public Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("fullscreen"))
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") != 0;
        }
    }
}

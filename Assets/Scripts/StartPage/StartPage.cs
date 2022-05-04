using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    // Start is called before the first frame update

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void LoadSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}

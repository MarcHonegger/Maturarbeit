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
        SceneManager.LoadScene("Game");
    }
    
    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}

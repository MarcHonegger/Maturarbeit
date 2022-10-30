using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject NetworkRoomID;
    
    private GameObject getObject()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name == "NetworkRoomManager")
            {
                return go;
            }
        }
        return null;
    }
    
    public void LoadGame()
    {
        try
        {
            NetworkRoomID = getObject();
            Debug.Log(NetworkRoomID);
        }
        catch
        {
            Debug.Log("coudn't find NetworkRoomID");
            Debug.Log(NetworkRoomID);
        }
        
        try
        {
            NetworkRoomID.SetActive(true);
        }
        catch
        {
            Debug.Log("couldn't SetActive");
        }
        
        SceneManager.LoadScene("OfflineScene");
    }
    
    public void LoadDeckBuilder()
    {
        SceneManager.LoadScene("DeckBuilderScene");
    }
    
    public void LoadSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("CreditsScene");
    }
}

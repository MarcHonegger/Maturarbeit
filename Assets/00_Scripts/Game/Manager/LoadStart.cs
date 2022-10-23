using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStart : NetworkBehaviour
{
    
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
    
    
    public void LoadStartPage()
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
            NetworkRoomID.SetActive(false);
        }
        catch
        {
            Debug.Log("couldn't SetActive");
        }

        SceneManager.LoadScene("StartPageScene");
    }
}

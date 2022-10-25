using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotooffline : NetworkBehaviour
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
    
    
    public void LoadRommScene()
    {

        stophost();
        stopclient();


    }

    [Server]
    public void stophost()
    {
        unload();
        Debug.Log("stophost");
        NetworkManager.singleton.StopHost();
    }

    [Client]
    public void stopclient()
    {
        unload();
        Debug.Log("stopclient");
        NetworkManager.singleton.StopClient();
    }


    public void unload()
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
            Debug.Log("SetActive(false)");
        }
        catch
        {
            Debug.Log("couldn't SetActive");
        }
    }
}

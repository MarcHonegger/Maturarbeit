using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLine : MonoBehaviour
{
    public NewPlayerManager newPlayerManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log("DeathLine");
            NetworkIdentity netID = NetworkClient.connection.identity;
            newPlayerManager = netID.GetComponent<NewPlayerManager>();
            newPlayerManager.GameOver();
        }
    }
}

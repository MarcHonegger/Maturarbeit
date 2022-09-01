using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLine : MonoBehaviour
{
    public NewPlayerManager newPlayerManager;

    private void Start()
    {
        if (gameObject.CompareTag("LeftPlayer"))
        {
            transform.position = new Vector3(-25, 0, 0);
        }
        else
        {
            transform.position = new Vector3(GameManager.instance.spawnManager.spawnPointDistance.x + 25,0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            int winner = 0;
            Debug.Log("DeathLine");
            NetworkIdentity netID = NetworkClient.connection.identity;
            newPlayerManager = netID.GetComponent<NewPlayerManager>();
            if (gameObject.tag == "LeftPlayer")
            {
                winner = 1;
            }
            newPlayerManager.RpcGameOver(winner);
            
        }
    }
}

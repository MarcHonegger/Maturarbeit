using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float tickRate;
    public bool clientStarted=false;
    public AudioManager audioManager;
    public SpawnManager spawnManager;
    public TroopManager troopManager;
    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        audioManager = GetComponentInChildren<AudioManager>();
        spawnManager = GetComponentInChildren<SpawnManager>();
        //NetworkIdentity netID = NetworkClient.connection.identity;
        //spawnManager = netID.GetComponent<SpawnManager>();
        troopManager = GetComponentInChildren<TroopManager>();
        //NetworkIdentity netID2 = NetworkClient.connection.identity;
        //troopManager = netID2.GetComponent<TroopManager>();
        
    }
    

   
}

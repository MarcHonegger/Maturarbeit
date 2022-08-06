using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioManager audioManager;
    public SpawnManager spawnManager;
    public TroopManager troopManager;
    public GameObject pauseMenu;
    
    public float tickRate;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

        // audioManager = GetComponentInChildren<AudioManager>();
        // spawnManager = GetComponentInChildren<SpawnManager>();
        //NetworkIdentity netID = NetworkClient.connection.identity;
        //spawnManager = netID.GetComponent<SpawnManager>();
        // troopManager = GetComponentInChildren<TroopManager>();
        //NetworkIdentity netID2 = NetworkClient.connection.identity;
        //troopManager = netID2.GetComponent<TroopManager>();
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        Debug.Log("Paused the Game");
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
}

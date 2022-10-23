using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public SpawnManager spawnManager;
    public TroopManager troopManager;
    public GameObject pauseMenu;
    
    public float tickRate;
    
    public InputAction pauseAction;

    private NetworkDiscoveryHUD _discoveryHUD;
    private NetworkRoomManager _networkRoomManager;
    
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
        _discoveryHUD = FindObjectOfType<NetworkDiscoveryHUD>();
        _networkRoomManager = FindObjectOfType<NetworkRoomManager>();
            
        _discoveryHUD.enabled = false;
        _networkRoomManager.showRoomGUI = false;
        
        pauseAction.performed += OnPause;
    }

    private void OnEnable()
        {
            pauseAction.Enable();
        }
    
    private void OnDisable()
        {
            pauseAction.Disable();
        }

    
    private void OnPause(InputAction.CallbackContext context)
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        _discoveryHUD.enabled = pauseMenu.activeSelf;
        FindObjectOfType<NetworkRoomManager>().showRoomGUI = pauseMenu.activeSelf;
    }
}

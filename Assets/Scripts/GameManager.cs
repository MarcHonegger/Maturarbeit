using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
        troopManager = GetComponentInChildren<TroopManager>();
    }
}

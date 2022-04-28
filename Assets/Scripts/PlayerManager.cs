using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float energy;
    public List<SpawnPoint> spawnPoints;
    
    [SerializeField] private float cooldownDuration;
    public float currentCooldown;
    
    public static PlayerManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    public bool CheckValidSpawn(int lane, bool isPlayerLeft)
    {
        return CheckSpawnAreas().Contains(spawnPoints[lane]);
    }

    private List<SpawnPoint> CheckSpawnAreas()
    {
        return spawnPoints.FindAll(s => !s.isDisabled);
    }

    public void CardPlayed(GameObject troopPrefab)
    {
        var troop = troopPrefab.GetComponent<Troop>();
        energy -= troop.energyCost;
        StartCooldown();
    }

    private void StartCooldown()
    {
        currentCooldown = cooldownDuration;
    }
}

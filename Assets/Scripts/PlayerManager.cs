using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float energy;
    
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

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            currentCooldown = 0;
        }
    }

    public bool CheckValidSpawn(int lane, bool isPlayerLeft)
    {
        return CheckSpawnAreas().Contains(GameManager.Instance.spawnManager.spawnPoints[lane]);
    }

    private List<SpawnPoint> CheckSpawnAreas()
    {
        return GameManager.Instance.spawnManager.spawnPoints.FindAll(s => !s.isDisabled);
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

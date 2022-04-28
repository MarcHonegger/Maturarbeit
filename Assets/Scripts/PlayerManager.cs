using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float energy;
    public List<SpawnPoint> spawnPoints;
    
    [SerializeField] private float cooldownDuration;
    public float currentCooldown;
    
    public List<bool> CheckSpawnAreas()
    {
        var validLanes = new List<bool>(spawnPoints.Count);

        foreach (var spawnPoint in spawnPoints)
        {
            var isValid = !spawnPoint.isDisabled;
            validLanes.Add(isValid);
            Debug.Log($"Lane is {isValid}" );
        }

        return validLanes;
    }

    private void Update()
    {
        CheckSpawnAreas();
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;

    // If a troop is in this area (around SpawnPoint), there can not be spawned another one
    public Vector3 spawnAreaSize;

    private void Start()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.gameObject.GetComponent<BoxCollider>().size = spawnAreaSize;
        }
    }

    public void Spawn(GameObject troopPrefab, int lane, bool isPlayerLeft)
    {
        var spawnPosition = spawnPoints[lane + (isPlayerLeft ? 0 : 4)].position;
        // SpawnPosition has to be shifted because the SpawnPoint of the troop is not 0|0|0
        var spawnOffset = troopPrefab.transform.GetChild(0).position;
        
        GameObject troopGameObject = Instantiate(troopPrefab, spawnPosition - spawnOffset, Quaternion.identity);
        troopGameObject.tag = isPlayerLeft ? "LeftPlayer" : "RightPlayer";
        
        troopGameObject.transform.RotateAround(troopGameObject.transform.GetChild(0).position, Vector3.right, 45);

    }
}

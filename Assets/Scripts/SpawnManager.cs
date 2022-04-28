using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;

    // If a troop is in this area (around Spawnpoint), there can not be spawned another one
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
        var troopGameObject = Instantiate(troopPrefab, spawnPoints[lane + (isPlayerLeft ? 0 : 4)].position, Quaternion.identity);
        troopGameObject.tag = isPlayerLeft ? "LeftPlayer" : "RightPlayer";
    }
}

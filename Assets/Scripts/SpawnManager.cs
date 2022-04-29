using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints;
    public int spawnPointAmount;
    public Vector3 spawnPointStart;
    public Vector3 playerDistance;
    public Vector3 spawnPointSpacing;
    public GameObject spawnPointPrefab;
    public GameObject spawnPointParent;

    // If a troop is in this area (around SpawnPoint), there can not be spawned another one
    public Vector3 spawnAreaSize;

    private void Start()
    {
        // Generates SpawnPoints
        spawnPointParent = new GameObject("SpawnPoints");
        var currentPosition = spawnPointStart;
        var currentPlayer = "left";
        for (int p = 0; p < 2; p++)
        {
            for (int s = 0; s < spawnPointAmount; s++)
            {
                var spawnPointGameObject = Instantiate(spawnPointPrefab, currentPosition, Quaternion.identity);

                spawnPointGameObject.transform.SetParent(spawnPointParent.transform);
                spawnPointGameObject.gameObject.GetComponent<BoxCollider>().size = spawnAreaSize;
                spawnPointGameObject.transform.Rotate(new Vector3(45, 0, 0));
                spawnPointGameObject.name = $"SpawnPoint {currentPlayer} {s}";
                spawnPoints.Add(spawnPointGameObject.GetComponent<SpawnPoint>());

                currentPosition += spawnPointSpacing;
            }

            currentPlayer = "right";
            currentPosition = spawnPointStart + playerDistance;
        }
    }

    public void Spawn(GameObject troopPrefab, int lane, bool isPlayerLeft)
    {
        var spawnPosition = spawnPoints[lane + (isPlayerLeft ? 0 : 4)].transform.position;
        // SpawnPosition has to be shifted because the SpawnPoint of the troop is not 0|0|0
        var spawnOffset = troopPrefab.transform.GetChild(0).position;

        GameObject troopGameObject = Instantiate(troopPrefab, spawnPosition - spawnOffset, Quaternion.identity);
        troopGameObject.tag = isPlayerLeft ? "LeftPlayer" : "RightPlayer";

        troopGameObject.transform.RotateAround(troopGameObject.transform.GetChild(0).position, Vector3.right, 45);
    }
}
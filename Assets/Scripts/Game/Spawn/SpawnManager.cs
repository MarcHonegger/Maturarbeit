using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public List<SpawnPoint> spawnPoints;
    public int spawnPointAmount;
    public Vector3 spawnPointStart;
    public Vector3 spawnPointDistance;
    public Vector3 spawnPointSpacing;
    public GameObject spawnPointPrefab;
    public GameObject spawnPointParent;
    public NewPlayerManager newPlayerManager;

    // If a troop is in this area (around SpawnPoint), there can not be spawned another one
    public Vector3 spawnAreaSize;

    private void Awake()
    {
        spawnPointParent = new GameObject("SpawnPoints");
        GenerateSpawnPoints("left", Vector3.zero);
        GenerateSpawnPoints("right", spawnPointDistance);
    }

    private void GenerateSpawnPoints(string player, Vector3 offset)
    {
        var currentPosition = spawnPointStart + offset;

        for (int s = 0; s < spawnPointAmount; s++)
        {
            var spawnPointGameObject = Instantiate(spawnPointPrefab, currentPosition, Quaternion.identity);

            spawnPointGameObject.transform.SetParent(spawnPointParent.transform);
            spawnPointGameObject.gameObject.GetComponent<BoxCollider>().size = spawnAreaSize;
            spawnPointGameObject.transform.Rotate(new Vector3(45, 0, 0));
            spawnPointGameObject.name = $"SpawnPoint {player} ({s})";
            spawnPoints.Add(spawnPointGameObject.GetComponent<SpawnPoint>());

            currentPosition += spawnPointSpacing;
        }
    }

    public void Spawn(GameObject troopPrefab, int lane, bool isLeftPlayer)
    {
        var spawnPosition = GetSpawnPoint(lane, isLeftPlayer).transform.position;
        // SpawnPosition has to be shifted because the SpawnPoint of the troop is not 0|0|0
        var spawnOffset = troopPrefab.transform.GetChild(0).position;

        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        newPlayerManager.CmdSpawn(troopPrefab.name, spawnPosition - spawnOffset, isLeftPlayer);
    }

    private SpawnPoint GetLeftSpawnPoint(int lane) => spawnPoints[lane];
    private SpawnPoint GetRightSpawnPoint(int lane) => spawnPoints[lane + spawnPointAmount];

    public SpawnPoint GetSpawnPoint(int lane, bool isLeftPlayer) => isLeftPlayer
        ? GetLeftSpawnPoint(lane)
        : GetRightSpawnPoint(lane);
}

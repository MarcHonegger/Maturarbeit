using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSpawnButton : MonoBehaviour
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
        spawnPointParent = new GameObject("SpawnPoints");
    }

    

    public void Spawn(GameObject troopPrefab, int lane, bool isLeftPlayer)
    {
        var spawnPosition = GetSpawnPoint(lane, isLeftPlayer).transform.position;

        GameObject troopGameObject = Instantiate(troopPrefab, spawnPosition, Quaternion.identity);
        troopGameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
    }

    private SpawnPoint GetLeftSpawnPoint(int lane) => spawnPoints[lane];
    private SpawnPoint GetRightSpawnPoint(int lane) => spawnPoints[lane + spawnPointAmount];

    public SpawnPoint GetSpawnPoint(int lane, bool isLeftPlayer) => isLeftPlayer
        ? GetLeftSpawnPoint(lane)
        : GetRightSpawnPoint(lane);
}

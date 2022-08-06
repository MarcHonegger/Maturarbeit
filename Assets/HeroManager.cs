using System.Collections;
using System.Collections.Generic;
using Mirror;
using networking;
using UnityEngine;

public class HeroManager : NetworkBehaviour
{
    public GameObject heroPrefab;
    public NewPlayerManager newPlayerManager;
    public float spawnPointGap;

    public override void OnStartServer()
    {
        Invoke(nameof(SpawnHeroes), 1f);
    }

    private void SpawnHeroes()
    {
        var spawnManager = GameManager.instance.spawnManager;
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        for (int i = 0; i < 4; i++)
        {
            newPlayerManager.CmdSpawn(heroPrefab.name, 
            spawnManager.spawnPointStart + (CompareTag("LeftPlayer") ? Vector3.zero : spawnManager.spawnPointDistance) + new Vector3(spawnPointGap * (CompareTag("LeftPlayer") ? 1 : -1), 0, i * spawnManager.spawnPointSpacing.z), 
            CompareTag("LeftPlayer"), 
            transform);
            // heroGameObject.transform.Rotate(new Vector3(45, 0, 0));
        }
    }
}

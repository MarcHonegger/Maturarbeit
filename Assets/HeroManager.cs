using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using networking;
using UnityEngine;

public class HeroManager : NetworkBehaviour
{
    [Serializable]
    public struct Regeneration
    {
        public float time;
        public float value;
    }
    
    public GameObject heroPrefab;
    public NewPlayerManager newPlayerManager;
    public float spawnPointGap;

    public float scaleTime;
    public float healthScale;
    public Regeneration healthRegeneration;

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
        InvokeRepeating(nameof(Scale), scaleTime, scaleTime);
        InvokeRepeating(nameof(Heal), healthRegeneration.time, healthRegeneration.time);
    }

    [Server]
    private void Scale()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<TroopHandler>().ChangeHealth(healthScale, false);
        }
    }

    [Server]
    private void Heal()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<TroopHandler>().ChangeHealth(healthRegeneration.value, true);
        }
    }
}

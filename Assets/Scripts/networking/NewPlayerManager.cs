using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NewPlayerManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> troopPrefabs;

    private readonly List<GameObject> _troops = new List<GameObject>();
    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");

        foreach (var troop in troopPrefabs)
        {
            _troops.Add(troop);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnStartClient()
    {
        base.OnStartClient();
        SetSide();
        Debug.Log("Client started");
    }

    [ClientRpc]
    public void RpcGameOver()
    {
        SceneManager.LoadSceneAsync("GameOverOverlay", LoadSceneMode.Single);
    }

    [Server]
    public void SetSide()
    {
        FindObjectOfType<PlayerManager>().isLeftPlayer = true;
    }

    //public void CmdSpawn(GameObject troopPrefab, int lane, bool isLeftPlayer)
    [Command]
    public void CmdSpawn(string troopName, Vector3 position, bool isLeftPlayer)
    {
        var toSpawn = Searching(_troops, troopName);
        toSpawn.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";

        var troopGameObject = Instantiate(toSpawn, position, Quaternion.identity);

        troopGameObject.transform.RotateAround(troopGameObject.transform.GetChild(0).position, Vector3.right, 45);
        NetworkServer.Spawn(troopGameObject);
        
        RpcTagging(troopGameObject, isLeftPlayer);
    }

    [Command]
    public void CmdSpawn2(GameObject objectToSpawn)
    {
        Debug.Log(objectToSpawn);
        NetworkServer.Spawn(objectToSpawn);
    }


    [ClientRpc]
    private void RpcTagging(GameObject troopinggameObject, bool isLeftPlayer)
    {
        troopinggameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
    }

    private GameObject Searching(List<GameObject> listing, string troopName)
    {
        return listing.Find(prefab => prefab.name == troopName);
    }
}

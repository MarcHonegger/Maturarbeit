using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
        Debug.Log("Client started");
    }

    //public void CmdSpawn(GameObject troopPrefab, int lane, bool isLeftPlayer)
    [Command]
    public void CmdSpawn(string troopName, Vector3 position, bool isLeftPlayer)
    {
        // Debug.Log("looking for: "+troopName);
        GameObject toSpawn = searching(_troops, troopName);
        GameObject troopGameObject = Instantiate(toSpawn, position, Quaternion.identity);
        NetworkServer.Spawn(troopGameObject);
        Rpctagging(troopGameObject, isLeftPlayer);
        RpcRotating(troopGameObject);
    }

    [Command]
    public void CmdSpawn2(GameObject objectToSpawn)
    {
        Debug.Log(objectToSpawn);
        NetworkServer.Spawn(objectToSpawn);
    }


    [ClientRpc]
    public void Rpctagging(GameObject troopinggameObject, bool isLeftPlayer)
    {
        troopinggameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
    }

    [ClientRpc]
    public void RpcRotating(GameObject troopinggameObject)
    {
        troopinggameObject.transform.RotateAround(troopinggameObject.transform.GetChild(0).position, Vector3.right, 45);
    }

    public GameObject searching(List<GameObject> listing, string troopName)
    {
        Debug.Log("listing name: " + listing[0].name);
        Debug.Log("Searching name " + troopName);
        for (int numbering = 0; numbering < listing.Count; numbering++)
        {
            string temp = listing[numbering].name;
            if (temp == troopName)
            {
                Debug.Log("Found :" + troopName + " in " + listing[numbering].name);
                return listing[numbering];
            }
        }
        throw new Exception("couldn't find anything");
    }

    [Command]
    public void CmdUpdateTag(GameObject troop)
    {   Debug.Log(troop);
        Debug.Log(troop.CompareTag("LeftPlayer"));
        if (gameObject.CompareTag("RightPlayer"))
        {
            //GetComponent<SpriteRenderer>().flipX = true;

            Debug.Log("before flip");
            RpcFlipX(gameObject);
        }
    }

    [Command]
    public void CmdDestroyTroop(GameObject troop)
    {
        Debug.Log(troop);
        NetworkServer.Destroy(troop.GetComponent<TroopHandler>().healthBar.gameObject);
        NetworkServer.Destroy(troop);
    }


    [ClientRpc]
    public void RpcFlipX(GameObject troop)
    {
        Debug.Log("should flip");
        troop.GetComponent<SpriteRenderer>().flipX = true;
    }

}

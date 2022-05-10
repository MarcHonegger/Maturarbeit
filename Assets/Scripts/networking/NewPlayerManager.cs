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
        Debug.Log("looking for: "+troopName);
        GameObject toSpawn = searching(_troops, troopName);
        GameObject troopGameObject = Instantiate(toSpawn, position, Quaternion.identity);
        NetworkServer.Spawn(troopGameObject);
        Rpctagging(troopGameObject, isLeftPlayer);
        RpcRotating(troopGameObject);
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
        Debug.Log("Searching name "+ troopName);
        for(int numbering = 0; numbering<listing.Count; numbering++)
        {
            string temp = listing[numbering].name;
            if (temp == troopName)
            {
                Debug.Log("Found :"+troopName +" in "+listing[numbering].name);
                return listing[numbering];
                
            }
        }
        throw new Exception("couldn't find anything");
    }
    
    
    [Command]
    public void CmdSpawn2(GameObject troop, int lane, bool isPlayerLeft)
    {
        GameObject troopGameObject = Instantiate(troop, new Vector3(-7.5f, 2, -2), Quaternion.Euler(45f, 0, 0));
        //troopGameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
        //troopGameObject.tag = "LeftPlayer";
        //GameObject troopGameObject2 = troopGameObject;
        NetworkServer.Spawn(troopGameObject, connectionToClient);
        //Debug.Log(troopGameObject);
        //Debug.Log(connectionToClient);

    }

    [Command]
    public void CmdUpdateTag(GameObject troop)
    {
        Debug.Log(troop.CompareTag("LeftPlayer"));
        if (gameObject.CompareTag("RightPlayer"))
        {
            //GetComponent<SpriteRenderer>().flipX = true;
                
            //Debug.Log("before flip");
            RpcFlipX(gameObject);
        }
    }

    [Command]
    public void CmdDestroyTroop(GameObject gameObject)
    {
        NetworkServer.Destroy(gameObject);
    }

   
    
    [ClientRpc]
    public void RpcFlipX(GameObject troop)
    {
        Debug.Log("should flip");
        troop.GetComponent<SpriteRenderer>().flipX = true;
    }
  
}

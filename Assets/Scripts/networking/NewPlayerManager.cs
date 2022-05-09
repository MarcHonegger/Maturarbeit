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
    public GameObject troopPrefab;
    public int lane = 2;
    private GameObject troopGameObject;
    
    [SerializeField] private GameObject Troop1;
    [SerializeField] private GameObject Troop2;
    [SerializeField] private GameObject Troop3;
    [SerializeField] private GameObject Troop4;
    [SerializeField] private GameObject Troop5;
    [SerializeField] private GameObject Troop6;
    [SerializeField] private GameObject Troop7;

    private List<GameObject> Troops = new List<GameObject>();
    // ReSharper disable Unity.PerformanceAnalysis
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
        
        Troops.Add(Troop1);
        Troops.Add(Troop2);
        Troops.Add(Troop3);
        Troops.Add(Troop4);
        Troops.Add(Troop5);
        Troops.Add(Troop6);
        Troops.Add(Troop7);
        
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
        GameObject toSpawn = searching(Troops, troopName);
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
    public void CmdSpawn2()
    {
        GameObject troopGameObject = Instantiate(troopPrefab, new Vector3(-7.5f, 2, -2), Quaternion.Euler(45f, 0, 0));
        //troopGameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
        //troopGameObject.tag = "LeftPlayer";
        //GameObject troopGameObject2 = troopGameObject;
        NetworkServer.Spawn(troopGameObject, connectionToClient);
        //Debug.Log(troopGameObject);
        //Debug.Log(connectionToClient);

    }

    [Command]
    public void CmdUpdateTag(GameObject gameObject)
    {
        Debug.Log(gameObject.CompareTag("LeftPlayer"));
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
    public void RpcFlipX(GameObject gameObject)
    {
        Debug.Log("should flip");
        gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }
  
}

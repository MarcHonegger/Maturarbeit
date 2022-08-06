using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using networking;
using UnityEngine;
using UnityEngine.Serialization;

public class newslimespawn : MonoBehaviour
{

    public NewPlayerManager newPlayerManager;
    public GameObject troopPrefab;
    public int lane = 2;
    public bool isLeftPlayer = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        //newSpawnButton.CmdSpawn(troopPrefab, lane, isLeftPlayer);
        NetworkIdentity netID = NetworkClient.connection.identity;
        newPlayerManager = netID.GetComponent<NewPlayerManager>();
        newPlayerManager.CmdSpawn2(troopPrefab);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}

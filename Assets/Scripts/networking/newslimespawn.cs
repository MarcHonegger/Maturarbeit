using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.Serialization;

public class newslimespawn : MonoBehaviour
{

    public NewPlayerManager newSpawnButton;
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
        newSpawnButton = netID.GetComponent<NewPlayerManager>();
        newSpawnButton.CmdSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

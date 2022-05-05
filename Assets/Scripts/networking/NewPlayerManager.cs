using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NewPlayerManager : NetworkBehaviour
{
    public GameObject troopPrefab;
    public int lane = 2;
    public bool isLeftPlayer = true;
    private GameObject troopGameObject;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client started");
    }
    
    //public void CmdSpawn(GameObject troopPrefab, int lane, bool isLeftPlayer)
    [Command]
    public void CmdSpawn()
    {
        GameObject troopGameObject = Instantiate(troopPrefab, new Vector3(-7.5f, 2, -2), Quaternion.Euler(45f, 0, 0));
        //troopGameObject.tag = isLeftPlayer ? "LeftPlayer" : "RightPlayer";
        //troopGameObject.tag = "LeftPlayer";
        //GameObject troopGameObject2 = troopGameObject;
        NetworkServer.Spawn(troopGameObject, connectionToClient);
        //Debug.Log(troopGameObject);
        //Debug.Log(connectionToClient);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

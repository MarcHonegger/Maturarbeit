using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NewPlayerManager : NetworkBehaviour
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Server started");
        
    }

    public void CreateSlime()
    {
        
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

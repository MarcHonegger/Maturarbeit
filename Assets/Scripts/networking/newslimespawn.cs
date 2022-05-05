using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newslimespawn : MonoBehaviour
{

    public NewSpawnButton testSpawnManager;
    public GameObject troopPrefab;
    public int lane = 2;
    public bool isLeftPlayer = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnClick()
    {
        testSpawnManager.Spawn(troopPrefab, lane, isLeftPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

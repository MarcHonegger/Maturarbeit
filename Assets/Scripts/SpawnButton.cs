using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject troopPrefab;
    public int lane;
    public bool isPlayerLeft;

    // Start is called before the first frame update
    void Start()
    {
        // _spawnPointTroop = troopPrefab.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        /*
        var spawnPosition = spawnPointPlayer.position;
        var spawnOffset = _spawnPointTroop.position;
        GameObject troop = Instantiate(troopPrefab, spawnPosition - spawnOffset, quaternion.identity);
        troop.transform.RotateAround(troop.transform.GetChild(0).position, Vector3.right, 45);
        */

        if (!PlayerManager.Instance.CheckValidSpawn(lane, isPlayerLeft))
        {
            return;
        }
        GameManager.Instance.spawnManager.Spawn(troopPrefab, lane, isPlayerLeft);
    }
}

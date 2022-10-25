using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject troopPrefab;
    public int lane;
    public bool isLeftPlayer;
    
    public void Click()
    {
        /*
        var spawnPosition = spawnPointPlayer.position;
        var spawnOffset = _spawnPointTroop.position;
        GameObject troop = Instantiate(troopPrefab, spawnPosition - spawnOffset, quaternion.identity);
        troop.transform.RotateAround(troop.transform.GetChild(0).position, Vector3.right, 45);
        */

        if (PlayerManager.instance.IsPlayableCard(troopPrefab.GetComponent<TroopHandler>().energyCost))
        {
            PlayerManager.instance.PlayCard(troopPrefab, lane, null);
        }
    }
}

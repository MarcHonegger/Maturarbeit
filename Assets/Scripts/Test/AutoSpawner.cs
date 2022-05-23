using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<int> lanes;
    public List<bool> isPlayerLeftList;
    public List<float> healthBuffs;
    

    // Start is called before the first frame update
    public void AutoSpawn()
    {
        var newPlayerManager = FindObjectOfType<NewPlayerManager>();
        for (int i = 0; i < gameObjects.Count; i++)
        {
            TroopHandler troop = gameObjects[i].GetComponent<TroopHandler>();
            troop.health += healthBuffs[i];
            newPlayerManager.CmdSpawn2(gameObjects[i]);
            troop.health -= healthBuffs[i];
        }
    }
}

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
        for (int i = 0; i < gameObjects.Count; i++)
        {
            TroopHandler troop = gameObjects[i].GetComponent<TroopHandler>();
            troop.health += healthBuffs[i];
            GameManager.instance.spawnManager.Spawn(gameObjects[i], lanes[i], isPlayerLeftList[i]);
            troop.health -= healthBuffs[i];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
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
            GameManager.Instance.spawnManager.Spawn(troop.gameObject, lanes[i], isPlayerLeftList[i]);
            troop.health -= healthBuffs[i];
        }
    }
}

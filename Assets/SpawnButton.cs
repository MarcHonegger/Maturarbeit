using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GameObject troop;
    public Transform spawnPointPlayer;
    private Transform _spawnPointTroop;

    // Start is called before the first frame update
    void Start()
    {
        _spawnPointTroop = troop.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        var spawnPosition = spawnPointPlayer.position;
        var spawnOffset = _spawnPointTroop.position;
        Debug.Log(spawnOffset);
        Instantiate(troop, spawnPosition - spawnOffset, Quaternion.identity);
    }
}

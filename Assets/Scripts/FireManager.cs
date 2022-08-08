using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class FireManager : MonoBehaviour
{
    public GameObject firePrefab;
    public float distance;
    public float duration;
    public float frequency;
    
    private readonly Queue<GameObject> _fireGameObjects = new Queue<GameObject>();
    

    private void Start()
    {
        SpawnFires();
        InvokeRepeating(nameof(ActivateNextFire), 0, frequency);
    }


    
    private Vector3 Direction() => (CompareTag("LeftPlayer") ? Vector3.right : Vector3.left);

    private void SpawnFires()
    {
        var spawnManager = GameManager.instance.spawnManager;
        var spawnPointDistanceX = spawnManager.spawnPointDistance.x;
        var amountNeeded = Mathf.Floor((spawnPointDistanceX * 0.85f)/ distance);
        // var startPosition = spawnManager.spawnPointStart + new Vector3(CompareTag("LeftPlayer") ? 0 : spawnPointDistanceX, transform.position.y, transform.position.z);
        var startPosition = transform.position;
        for (int i = 0; i < amountNeeded; i++)
        {
            var position = startPosition + Direction() * distance * i;
            var fireGameObject = Instantiate(firePrefab, position, Quaternion.identity, transform);
            fireGameObject.SetActive(false);
            fireGameObject.transform.Rotate(new Vector3(45, 0, 0));
            _fireGameObjects.Enqueue(fireGameObject);
        }
    }

    private void ActivateNextFire()
    {
        if(transform.childCount == 0)
            Destroy(gameObject);
        if (!_fireGameObjects.Any())
            return;
        _fireGameObjects.Peek().SetActive(true);
        Destroy(_fireGameObjects.Peek(), duration);
        _fireGameObjects.Dequeue();
    }
}

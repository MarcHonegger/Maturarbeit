using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTime : MonoBehaviour
{

    [SerializeField] private float delay = 10f;

    [SerializeField] private string sceneToLoad;
    
    private float _timeElapsed;
    
    void FixedUpdate()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= delay)
        { 
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

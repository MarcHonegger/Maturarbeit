using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waitForTime : MonoBehaviour
{
    private IEnumerator coroutine;

    void Start()
    {
        // - After 0 seconds, prints "Starting 0.0 seconds"
        // - After 0 seconds, prints "Coroutine started"
        // - After 2 seconds, prints "Coroutine ended: 2.0 seconds"
        print("Starting " + Time.time + " seconds");

        // Start function WaitAndPrint as a coroutine.

        coroutine = WaitAndPrint(2.0f);
        StartCoroutine(coroutine);

        print("Coroutine started");
    }

    public void someFunction()
    {
        
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");
    }
}
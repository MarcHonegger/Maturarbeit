using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaling : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(0.1f + Screen.width / 1920f, 0.1f + Screen.height / 1080f, 1);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isDisabled;
    public Color normalColor;
    public Color disabledColor;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        isDisabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = disabledColor;
    }

    private void OnTriggerExit(Collider other)
    {
        isDisabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = normalColor;
    }
}

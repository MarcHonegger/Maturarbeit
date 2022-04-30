using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isDisabled;
    public Color normalColor;
    public Color disabledColor;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _spriteRenderer.color = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        isDisabled = true;
        _spriteRenderer.color = disabledColor;
    }

    private void OnTriggerExit(Collider other)
    {
        isDisabled = false;
        _spriteRenderer.color = normalColor;
    }
}

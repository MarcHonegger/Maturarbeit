using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isDisabled;
    // public Color normalColor;
    // public Color disabledColor;
    private SpriteRenderer _spriteRenderer;
    private int _troopCount;
    private Animator _animator;
    private static readonly int Close = Animator.StringToHash("Close");
    private static readonly int Open = Animator.StringToHash("Open");

    private void Start()
    {
        // _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        // _spriteRenderer.color = normalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 6)
            return;

        _troopCount++;
        ClosePortal();
        other.GetComponent<TroopHandler>().Death += TroopOutOfRange;
        // _spriteRenderer.color = disabledColor;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer != 6)
            return;
        TroopOutOfRange();
    }

    private void TroopOutOfRange()
    {
        if (_troopCount > 1)
            return;
        _troopCount = 0;
        OpenPortal();
        // _spriteRenderer.color = normalColor;
    }
    
    private void OpenPortal()
    {
        isDisabled = false;
        _animator.SetTrigger(Open);
    }
    
    private void ClosePortal()
    {
        isDisabled = true;
        _animator.SetTrigger(Close);
    }
}

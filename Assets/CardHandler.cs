using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum CardType
{
    Troop = 0, Spell = 1
}

public class CardHandler : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject cardGameObject;
    public int cost;
    public CardType type;

    /// <summary>
    /// Drag and Drop for green cards
    /// </summary>
    
    [SerializeField] private Canvas canvas;
    private RectTransform _rectTransform;
    private bool disabled;
    private Image _image;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!PlayerManager.instance.IsPlayableCard(cost))
        {
            _image.color = Color.gray;
            disabled = true;
        }
        else
        {
            _image.color = Color.white;
            disabled = false;
        }
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (disabled)
        {
            eventData.pointerDrag = null;
            return;
        }
        Debug.Log($"BeginDrag");
        HandManager.instance.StartRendering();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag");
        HandManager.instance.StopRendering();
        HandManager.instance.CheckPosition(Mouse.current.position.ReadValue(), cardGameObject, gameObject);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Drag");
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down");
    }
}

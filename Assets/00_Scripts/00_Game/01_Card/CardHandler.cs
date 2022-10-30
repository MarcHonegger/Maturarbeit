using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
//using UnityEditor.UI;
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

    // Card Infos
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI typeText;
    public Image troopImage;
    public Image attackTypeImage;

    public bool isDragged;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private bool _disabled;
    private Image _image;
    private Vector3 _normalScale;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _normalScale = transform.localScale;
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _canvas = FindObjectOfType<Canvas>();
        SetCardInfos();
    }

    private void SetCardInfos()
    {
        var troop = cardGameObject.GetComponent<TroopHandler>();
        healthText.text = troop.maximumHealth.ToString(CultureInfo.InvariantCulture);
        cost = troop.energyCost;
        costText.text = cost.ToString(CultureInfo.InvariantCulture);
        nameText.text = troop.troopName;
        typeText.text = troop.type;
        troopImage.preserveAspect = true;
        troopImage.sprite = cardGameObject.GetComponent<SpriteRenderer>().sprite;
        attackTypeImage.sprite = Resources.Load<Sprite>($"Game/{troop.attackType}");
    }

    public void PlayCardIsDrawnAnimation()
    {
        transform.localScale = 0.75f * Vector3.one;
        InvokeRepeating(nameof(ScaleUp01), 0.02f, 0.02f);
    }

    private void ScaleUp01()
    {
        transform.localScale += 0.125f * Vector3.one;
        if (transform.localScale.x >= _normalScale.x + 0.3f)
        {
            transform.localScale = _normalScale;
            CancelInvoke(nameof(ScaleUp01));
        }
    }

    private void Update()
    {
        if (!PlayerManager.instance.IsPlayableCard(cost))
        {
            _image.color = Color.red;
            _disabled = true;
        }
        else
        {
            _image.color = Color.white;
            _disabled = false;
        }
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_disabled)
        {
            eventData.pointerDrag = null;
            return;
        }
        Debug.Log($"BeginDrag");
        HandManager.instance.StartRendering();
        isDragged = true;
        // HandManager.instance.DragCard(gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag");
        HandManager.instance.StopRendering();
        HandManager.instance.CheckPosition(Mouse.current.position.ReadValue(), cardGameObject, gameObject);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"Drag");
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        // HandManager.instance.DragCard(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down");
    }
}

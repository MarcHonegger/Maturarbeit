using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragNDrop2 : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// Drag and Drop for green cards
    /// </summary>
    private RectTransform _rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;
    private Vector3 _oldPos;
    public GameObject laneOptionPrefab;
    private readonly List<Image> _laneSprites = new List<Image>();
    private readonly List<TextMeshProUGUI> _laneNumbers = new List<TextMeshProUGUI>();
    /*
    [SerializeField] private Image sprite1;
    [SerializeField] private Image sprite2;
    [SerializeField] private Image sprite3;
    [SerializeField] private Image sprite4;
    */
    [SerializeField] private GameObject troopPrefab;

    public bool currentPlayer;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"BeginDrag");
        EnableRendering();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag");
        //Debug.Log("transform.position" + transform.position);
        transform.position = _oldPos;
        DisableRendering();
        CheckPosition();
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
    
    void Start()
    {
        cam = Camera.main;
        _oldPos = transform.position;

        GenerateSprites();
    }

    private void GenerateSprites()
    {
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;
        int thirdScreenHeight = screenHeight / 3;
        int quarterScreenWidth = screenWidth / 4;
        Vector3 laneOptionScale = new Vector3(quarterScreenWidth / 100f, thirdScreenHeight * 2 / 100f, 1);

        var laneOptionParent = GameObject.Find("laneOptionOverlay").transform;
        for (int i = 0; i < 4; i++)
        {
            var laneOption = Instantiate(laneOptionPrefab, new Vector3(quarterScreenWidth / 2f + quarterScreenWidth * i, thirdScreenHeight * 2f, 0f), quaternion.identity, laneOptionParent);
            laneOption.transform.localScale = laneOptionScale;
            var laneNumber = laneOption.GetComponentInChildren<TextMeshProUGUI>();
            laneNumber.text = (i + 1).ToString();
            var laneOptionImage = laneOption.GetComponent<Image>();
            
            laneOptionImage.enabled = false;
            laneNumber.enabled = false;
            _laneSprites.Add(laneOptionImage);
            _laneNumbers.Add(laneNumber);
        }
    }

    public void DisableRendering()
    {   
        //disables rendering of the 4 overlay numbers
        foreach (var sprite in _laneSprites)
        {
            sprite.enabled = false;
        }
        foreach (var number in _laneNumbers)
        {
            number.enabled = false;
        }
    }

    public void EnableRendering()
    {
        //enables rendering of the 4 overlay numbers
        foreach (var sprite in _laneSprites)
        {
            sprite.enabled = true;
        }
        foreach (var number in _laneNumbers)
        {
            number.enabled = true;
        }
    }

    public void ChangeCurrentPlayer()
    {
        currentPlayer = !currentPlayer;
    }

    public void CheckPosition()
    {
        int screenHeight = Screen.height;
        int screenWidth = Screen.width;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Debug.Log("mouse position: " + mousePosition);
        int thirdScreenHeight = screenHeight / 3;
        int quarterScreenWidth = screenWidth / 4;
        if (mousePosition.y > thirdScreenHeight)
        {
            if (mousePosition.x < quarterScreenWidth)
            {
                PlayerManager.instance.PlayCard(troopPrefab, 0, gameObject);
            } 
            else if (mousePosition.x > quarterScreenWidth && mousePosition.x < 2 * quarterScreenWidth)
            {
                PlayerManager.instance.PlayCard(troopPrefab, 1, gameObject);
            }
            else if (mousePosition.x > 2 * quarterScreenWidth && mousePosition.x < 3 * quarterScreenWidth)
            {
                PlayerManager.instance.PlayCard(troopPrefab, 2, gameObject);
            }
            else if (mousePosition.x > 3 * quarterScreenWidth && mousePosition.x < 4 * quarterScreenWidth)
            {
                PlayerManager.instance.PlayCard(troopPrefab, 3, gameObject);
            }
        }
        transform.position = _oldPos;
    }
}

    

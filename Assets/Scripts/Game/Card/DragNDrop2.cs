using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Image sprite1; 
    [SerializeField] private Image sprite2; 
    [SerializeField] private Image sprite3; 
    [SerializeField] private Image sprite4;
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
    }

    public void DisableRendering()
    {   
        //disables rendering of the 4 overlay numbers
        sprite1.enabled = false;
        sprite2.enabled = false;
        sprite3.enabled = false;
        sprite4.enabled = false;
    }

    public void EnableRendering()
    {
        //enables rendering of the 4 overlay numbers
        sprite1.enabled = true;
        sprite2.enabled = true;
        sprite3.enabled = true;
        sprite4.enabled = true;
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
        int halfScreenHeight = screenHeight / 2;
        int halfScreenWidth = screenWidth / 2;
        if (mousePosition.y > halfScreenHeight && mousePosition.x < halfScreenWidth)
        {
            //1
            Debug.Log("1");
            GameManager.Instance.spawnManager.Spawn(troopPrefab, 0, currentPlayer);
        } 
        else if (mousePosition.y > halfScreenHeight && mousePosition.x > halfScreenWidth)
        {
            //2
            Debug.Log("2");
            GameManager.Instance.spawnManager.Spawn(troopPrefab, 1, currentPlayer);
        }
        else if (mousePosition.y < halfScreenHeight && mousePosition.x < halfScreenWidth)
        {
            //3
            Debug.Log("3");
            GameManager.Instance.spawnManager.Spawn(troopPrefab, 2, currentPlayer);
        }
        else if (mousePosition.y < halfScreenHeight && mousePosition.x > halfScreenWidth)
        {
            //4
            Debug.Log("4");
            GameManager.Instance.spawnManager.Spawn(troopPrefab, 3, currentPlayer);
        }
        transform.position = _oldPos;
    }
}

    

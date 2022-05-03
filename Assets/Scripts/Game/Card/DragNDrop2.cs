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
        DisableRendering();
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
    

    public void CheckPosition()
    {
        int ScreenHeight = Screen.height;
        int ScreenWidth = Screen.width;
        Vector3 MousePosition = Mouse.current.position.ReadValue();
        Debug.Log("mouse position: " + MousePosition);
        if (MousePosition.y > ScreenHeight/2 && MousePosition.x < ScreenWidth/2)
        {
            //1
            Debug.Log("1");
        } 
        else if (MousePosition.y > ScreenHeight/2 && MousePosition.x > ScreenWidth/2)
        {
            //2
            Debug.Log("2");
        }
        else if (MousePosition.y < ScreenHeight / 2 && MousePosition.x < ScreenWidth / 2)
        {
            //3
            Debug.Log("3");
        }
        else if (MousePosition.y < ScreenHeight / 2 && MousePosition.x > ScreenWidth / 2)
        {
            //4
            Debug.Log("4");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

}

    

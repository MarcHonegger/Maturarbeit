using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        UnityEngine.Debug.Log($"BeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UnityEngine.Debug.Log($"EndDrag");
        UnityEngine.Debug.Log($"{rectTransform.position}");
    }
    public void OnDrag(PointerEventData eventData)
    {
        UnityEngine.Debug.Log($"Drag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UnityEngine.Debug.Log($"Down");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

    

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;
    [SerializeField] public GameObject spawnPointPrefab;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"BeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag");
        
        Vector3 point = new Vector3();
        Event   currentEvent = Event.current;
        Vector2 mousePos = new Vector2();
        
        
        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        //int layerMask = 1 << 7;
        //layerMask = ~layerMask;
        Transform transform = cam.transform;
        LayerMask layermask = 1 << 7;
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        //Debug.Log(ray);
        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        //Debug.Log(point);
        if (Physics.Raycast(ray, out hit,  Mathf.Infinity, layermask))
        {
            Vector3 worldPoint = hit.point;
            var xpoint = worldPoint.y;
            //Debug.Log(hit.point);
            Instantiate(spawnPointPrefab, worldPoint, Quaternion.Euler(45f, 0f, 0f));
        }

        
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Drag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Down");
    }
    void Start()
    {
        
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {

    }

}

    

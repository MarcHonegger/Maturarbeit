using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragNDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// Drag and Drop for the red cards
    /// </summary>
    private RectTransform _rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject spawnPointPrefab;
    private Vector3 _oldPos;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"BeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"EndDrag");
        LayerMask layermask = 1 << 7;
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit,  Mathf.Infinity, layermask))
        {
            Vector3 worldPoint = hit.point;
            Instantiate(spawnPointPrefab, worldPoint, Quaternion.Euler(45f, 0f, 0f));
            transform.position = _oldPos;
        }
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

    // Update is called once per frame
    void Update()
    {

    }

}

    

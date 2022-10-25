using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleoverlay : MonoBehaviour
{
    /// <summary>
    /// idk ob ich das hier noch brauch
    /// </summary>
    // [SerializeField] private Sprite sprite1; 
    // [SerializeField] private Sprite sprite2;
    // [SerializeField] private Sprite sprite3; 
    // [SerializeField] private Sprite sprite4; 
    //
    
    // Start is called before the first frame update
    void Start()
    {
        var rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = false;
    }

    private void _start_rend()
    {
        var rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = true;
    }
    private void _stop_rend()
    {
        var rend = gameObject.GetComponent<SpriteRenderer>();
        rend.enabled = false;
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}

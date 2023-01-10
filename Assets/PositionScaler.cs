using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionScaler : MonoBehaviour
{
    public Vector2 baseResolution;
    
    void Start()
    {
        var t = transform;
        var position = t.localPosition;
        var localScale = t.parent.localScale;
        t.localPosition = new Vector3(Screen.width / baseResolution.x * position.x / localScale.x,  Screen.height * position.y / baseResolution.y / localScale.y, 0);
    }
}

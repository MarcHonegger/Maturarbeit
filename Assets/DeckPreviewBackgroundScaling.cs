using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPreviewBackgroundScaling : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(Mathf.Ceil(100 * Screen.width / 1920f) / 100, Mathf.Ceil(100 * Screen.height / 1080f) / 100, 1);
    }
}

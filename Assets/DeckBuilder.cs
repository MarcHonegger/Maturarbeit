using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    public GameObject cardPreviewPrefab;

    // Start is called before the first frame update
    void Start()
    {
        var cardPreview = Instantiate(cardPreviewPrefab, transform);
        cardPreview.transform.localPosition = new Vector3(575, 325, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

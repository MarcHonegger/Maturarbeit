using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTest : MonoBehaviour
{
    public Color readyColor;
    public Color onCooldownColor;
    private Image _image;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = readyColor;
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _image.color =
            PlayerManager.instance.currentCooldown > 0 ? onCooldownColor : readyColor;
    }
}
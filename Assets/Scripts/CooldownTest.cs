using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTest : MonoBehaviour
{
    public Color readyColor;
    public Color onCooldownColor;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = readyColor;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color =
            PlayerManager.Instance.currentCooldown > 0 ? onCooldownColor : readyColor;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerTest : MonoBehaviour
{
    public void ChangeCurrentPlayer()
    {
        foreach (var child in GetComponentsInChildren<DragNDrop2>())
        {
            child.ChangeCurrentPlayer();
        }
    }
}

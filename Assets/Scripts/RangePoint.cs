using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangePoint : MonoBehaviour
{
   private BoxCollider _collider;
   public float ySize;
   public float zSize;
   
   private void Awake()
   {
      _collider = GetComponent<BoxCollider>();
   }

   public void UpdateRange(float range)
   {
      _collider.size = new Vector3(range, ySize, zSize);
      var offset = PlayerManager.Instance.GetDirection(transform.parent.gameObject) * range / 2;
      _collider.center = new Vector3(offset, 0, 0);
   }
}

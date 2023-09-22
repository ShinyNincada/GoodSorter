using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 ){
            GameObject dropped = eventData.pointerDrag;
            ToyObject dragableItem = dropped.GetComponent<ToyObject>();
            dragableItem.SetParentTransform(this.transform);            
            // Debug.Log(dragableItem.transform.parent);
        }
    }

    public void DestroyItem(){
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}


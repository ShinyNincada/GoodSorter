using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public class OnAnyItemDroppedArgs : EventArgs{
        public ToyObject _toyObject;

        public OnAnyItemDroppedArgs(ToyObject toyObject) {
            _toyObject = toyObject;
        }
    }
    
    public event EventHandler<OnAnyItemDroppedArgs> OnAnyItemDropped;

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 ){
            GameObject dropped = eventData.pointerDrag;
            ToyObject dragableItem = dropped.GetComponent<ToyObject>();
            dragableItem.SetParentTransform(this.transform);
            
            
            OnAnyItemDropped?.Invoke(this, new OnAnyItemDroppedArgs (dragableItem));
        }
    }

    public void DestroyItem(){
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}


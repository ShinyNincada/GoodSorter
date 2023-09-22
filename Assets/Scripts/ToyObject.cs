using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToyObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public class OnAnyItemDroppedArgs : EventArgs{
        public ToyObject _toyObject;

        public OnAnyItemDroppedArgs(ToyObject toyObject) {
            _toyObject = toyObject;
        }
    }
    public static event EventHandler<OnAnyItemDroppedArgs> OnAnyItemDropped;

    public SlotHolder lastHolder;
    public SlotHolder currentHolder;
    [SerializeField] private ToyObjectSO toySO; 
    [HideInInspector] Transform parentTransform;
    public Image image;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform = this.transform.parent;
        lastHolder = parentTransform.GetComponentInParent<SlotHolder>();
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;
        // Set the z component to the distance between the object and the camera
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        
        // Convert the screen coordinates to world coordinates
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Ignore the z-axis by keeping the original z value
        worldPosition.z = transform.position.z;

        // Update the object's position
        transform.position = worldPosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransform);
        currentHolder = parentTransform.GetComponentInParent<SlotHolder>();
        image.raycastTarget = true;
        
        OnAnyItemDropped?.Invoke(this, new OnAnyItemDroppedArgs (this));

    }

    public void SetParentTransform(Transform newParent){
        parentTransform = newParent;
    }

    public ToyObjectSO GetToyObjectSO() {
        return toySO;
    }
}

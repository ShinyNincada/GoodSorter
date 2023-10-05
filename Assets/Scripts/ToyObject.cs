using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToyObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   

    public BaseSlot lastHolder;
    public BaseSlot currentHolder;
    [SerializeField] private ToyObjectSO toySO; 
    protected Transform parentTransform;
    public Image image;
    [SerializeField] Collider _collider;



    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        parentTransform = this.transform.parent;
        lastHolder = parentTransform.GetComponentInParent<SlotHolder>();
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        //Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    public virtual void OnDrag(PointerEventData eventData)
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

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentTransform);
        currentHolder = parentTransform.GetComponentInParent<SlotHolder>();
        image.raycastTarget = true;

        if (currentHolder == lastHolder)
        {
            //If both new and old holder is the same, do nothing
            return;
        }

        // Add the toy dropped to the newSlot first to prevent the case that shelf have no item
        lastHolder.SetActiveToyObject(null);
        lastHolder.RemoveFirstToyFromList();
        currentHolder.AddNewToy(this);
        currentHolder.SetActiveToyObject(this);

        // If we moving to different shelf so try to sort
        // Dont need to sort if moving in the same shelf
        if (currentHolder.GetToyShelf() != lastHolder.GetToyShelf())
        {
            SortingManager.Instance.TrySortingToy(currentHolder.GetToyShelf());
            SortingManager.Instance.TrySortingToy(lastHolder.GetToyShelf());
        }
    }

    public void SetParentTransform(Transform newParent){
        parentTransform = newParent;
    }

    public ToyObjectSO GetToyObjectSO() {
        return toySO;
    }
}

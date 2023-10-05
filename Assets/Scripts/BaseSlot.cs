using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseSlot : MonoBehaviour, IToyObjectParent, IHasToyObjectList, IDropHandler
{
    public class OnActiveToyChangedArgs : EventArgs
    {
        public ToyObject newToy;

        public ToyObject oldToy;


        public OnActiveToyChangedArgs(ToyObject newToy)
        {
            this.newToy = newToy;
        }

        public OnActiveToyChangedArgs (ToyObject newToy, ToyObject oldToy)
        {
            this.newToy = newToy;
            this.oldToy = oldToy;
        }

       
    }
    public static event EventHandler<OnActiveToyChangedArgs> OnActiveToyChanged;

    [SerializeField] ToyShelf shelf;
    [SerializeField] protected Image backItem;
    [SerializeField] protected Transform frontItem;

    [SerializeField] List<ToyObject> toyList = new List<ToyObject>();
    [SerializeField] private ToyObject activeToy;

    [SerializeField] private bool isDropable = true;

   

    public Transform GetToyObjectHolder()
    {
        return frontItem;
    }


    public bool HasActiveToyObject()
    {
        return activeToy != null;
    }

    public ToyObject GetActiveToyObject()
    {
        return activeToy;
    }

    public void SetActiveToyObject(ToyObject obj)
    {
        ToyObject old = activeToy;
        activeToy = obj;

        if (obj == null)
        {
            OnActiveToyChanged?.Invoke(this, new OnActiveToyChangedArgs(obj, old));
        }
        else
        {
            if(GetToyShelf() is Blocked_Shelf)
            {
                var blockShelf = GetToyShelf() as Blocked_Shelf;
                if(!blockShelf.IsBlocking)
                {
                    OnActiveToyChanged?.Invoke(this, new OnActiveToyChangedArgs(obj, obj));
                }
            }
            else
            {
                OnActiveToyChanged?.Invoke(this, new OnActiveToyChangedArgs(obj));
            }  
        } 
    }

   

    public void AddNewToy(ToyObject newToy)
    {
        toyList.Insert(0, newToy);
    }

    public ToyObject GetFirstToy()
    {
        return toyList[0];
    }
    
    public ToyObject GetSecondToy()
    {
        return toyList[1];
    }


    
    public void RemoveFirstToyFromList()
    {
        toyList.RemoveAt(0);
    }

    public List<ToyObject> GetToyList()
    {
        return toyList;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (frontItem.childCount == 0 && isDropable)
        {
            GameObject dropped = eventData.pointerDrag;
            ToyObject dragableItem = dropped.GetComponent<ToyObject>();
            dragableItem.SetParentTransform(frontItem);
        }
    }

    public void DestroyItem()
    {
        foreach (Transform child in frontItem)
        {
            //debug.log("destroyed: " + child.gameobject);
            Destroy(child.gameObject);
        }

    }

    public ToyShelf GetToyShelf()
    {
        return shelf;
    }

    public bool GetIsDropable()
    {
        return isDropable;
    }

    public void SetIsDropable(bool canDrop)
    {
        isDropable = canDrop;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingManager : MonoBehaviour
{
    public SortingManager Instance;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ToyObject.OnAnyItemDropped += ToyObject_OnAnyItemDropped;
    }

    private void ToyObject_OnAnyItemDropped(object sender, ToyObject.OnAnyItemDroppedArgs e)
    {
        // Debug.Log(e._toyObject.transform.parent.childCount);
        if(e._toyObject.currentHolder == e._toyObject.lastHolder) {
            return;
        }
        if(e._toyObject.currentHolder != e._toyObject.lastHolder) {
            e._toyObject.lastHolder.ClearActiveToy();
            e._toyObject.lastHolder.RemoveFirstToy();

            e._toyObject.lastHolder.shelf.TrySortingToy();
        }

        e._toyObject.currentHolder.AddNewToy(e._toyObject);
        e._toyObject.currentHolder.SetActiveToy(e._toyObject);

        e._toyObject.currentHolder.shelf.TrySortingToy();
    }

   
}

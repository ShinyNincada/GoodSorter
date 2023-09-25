using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotHolder : MonoBehaviour
{
    public static event  EventHandler OnActiveItemChanged;
    [SerializeField] ToyShelf shelf;
    [SerializeField] Image backItem;
    [SerializeField] ItemSlot frontItem;

    [SerializeField] List<ToyObject> toyList = new List<ToyObject>();
    [SerializeField] private ToyObject activeToy;
    
    private void Start() {
        if(toyList.Count >= 2) {
            if(toyList[0] != null) {
                var newToy = SpawnNewToy(toyList[0]);
                SetActiveToy(newToy);
            }
            backItem.sprite = toyList[1].GetToyObjectSO().sprite;
        }
        else if(toyList.Count == 1) {
            var newToy = SpawnNewToy(toyList[0]);
            SetActiveToy(newToy);
        }
        else {
            return;
        }
    }

    private void ToyObject_OnAnyItemDropped(object sender, ToyObject.OnAnyItemDroppedArgs e)
    {
        if(e._toyObject.currentHolder == this) {
            return;
        }
        if(e._toyObject.currentHolder != this) {
            e._toyObject.currentHolder.ClearActiveToy();
            e._toyObject.currentHolder.RemoveFirstToy();
        }

        AddNewToy(e._toyObject);
        SetActiveToy(e._toyObject);

        shelf.TrySortingToy();

    }

    public void SetActiveToy(ToyObject newToy){
        activeToy = newToy;

        OnActiveItemChanged?.Invoke(this, EventArgs.Empty);
    }

    public ToyObject GetActiveToy(){
        return activeToy;
    }

    public bool HasActiveToy(){
        return activeToy != null;
    }
    public void ClearActiveToy(){
        activeToy = null;
    }

    public void AddNewToy(ToyObject newToy) {
        toyList.Insert(0, newToy);
    }

    public void RemoveFirstToy(){
        toyList.RemoveAt(0);
    }

    public List<ToyObject> GetToyList(){
        return toyList;
    }

    public ToyObject SpawnNewToy(ToyObject newToyObject){
        var newToy = Instantiate(newToyObject, frontItem.transform);
        newToy.SetParentTransform(frontItem.transform);
        return newToy;
    }

    public void PushNewToyOut(){
        if(toyList.Count > 0) {
            if(toyList[0] == null) {
                toyList.RemoveAt(0);
            }
            var toySpawned = SpawnNewToy(toyList[0]);
            SetActiveToy(toySpawned);
        }
        if(toyList.Count >= 2) {
            backItem.sprite = toyList[1].GetToyObjectSO().sprite;
        }
        else {
            backItem.sprite = null;
        }
    }

    public void ClearSortedToys(){
        ClearActiveToy();
        RemoveFirstToy();
        frontItem.DestroyItem();
        PushNewToyOut();
    }

    public ToyShelf GetToyShelf(){
        return shelf;
    }
}

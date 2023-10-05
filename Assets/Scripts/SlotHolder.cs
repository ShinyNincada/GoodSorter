using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHolder : BaseSlot
{
    private void Start()
    {
        if (GetToyList().Count >= 2)
        {
            if (GetFirstToy() != null)
            {
                var newToy = SpawnNewToy(GetFirstToy());
                SetActiveToyObject(newToy);
            }
            SetBackupToy(GetSecondToy().GetToyObjectSO());
        }
        else if (GetToyList().Count == 1)
        {
            if (GetFirstToy() != null)
            {
                var newToy = SpawnNewToy(GetFirstToy());
                SetActiveToyObject(newToy);
            }
        }
        else
        {
            return;
        }
    }


    public ToyObject SpawnNewToy(ToyObject newToyObject){
        var newToy = Instantiate(newToyObject, frontItem.transform);
        Debug.Log("Spawned: " + newToy);
        newToy.SetParentTransform(frontItem.transform);
        return newToy;
    }

    public void PushNewToyOut(){
        if(GetToyList().Count > 0) {
            if(GetFirstToy() == null) {
                RemoveFirstToyFromList();
            }
            var toySpawned = SpawnNewToy(GetFirstToy());
            SortingManager.Instance.waitingToyLists.Remove(GetFirstToy().GetToyObjectSO());
            SetActiveToyObject(toySpawned);
        }
        if(GetToyList().Count >= 2) {
            SetBackupToy(GetSecondToy().GetToyObjectSO());
        }
        else {
            backItem.sprite = null;
        }
    }

    public void ClearSortedToys(){
        SetActiveToyObject(null);
        RemoveFirstToyFromList();
        frontItem.transform.DOScale(1.3f, 1f).OnComplete(() =>  {
            DestroyItem();
            PushNewToyOut();
            frontItem.transform.localScale = Vector3.one;
        }
        );
    }

    public void ClearPickupToy()
    {
        SetActiveToyObject(null);
        RemoveFirstToyFromList();
        frontItem.transform.DOScale(1.3f, 1f).OnComplete( async () => {
            DestroyItem();
            frontItem.transform.localScale = Vector3.one;
            await Task.Delay( 100 );
            SortingManager.Instance.TrySortingToy(GetToyShelf());
        }
        );
    }

    public void SetBackupToy(ToyObjectSO objectSO)
    {
        if(objectSO != null)
        {
            backItem.sprite = objectSO.sprite;
            SortingManager.Instance.waitingToyLists.Add(objectSO);
        }
        
    }

    
}

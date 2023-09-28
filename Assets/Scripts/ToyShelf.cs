using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToyShelf : MonoBehaviour
{
    public static event EventHandler OnAnyToyShorted;
    [SerializeField] List<SlotHolder> slotHolders = new List<SlotHolder>(3);
    [SerializeField] bool sortable;

    public SortingType IsSortable(){
        int count = 0;
        int sameCount = 0;
        ToyObjectSO currentToy = null;
        foreach(SlotHolder slot in slotHolders){
            if(!slot.HasActiveToy()) {
                // If have no active toy
                if(currentToy != null) {
                    // if checked a toy in the shelf before
                    return SortingType.NONE;
                }
                else {
                    count++;
                }
            }
            else {
                // If slot has an active toy
                if(currentToy == null) {
                    //If current toy is null
                    currentToy = slot.GetActiveToy().GetToyObjectSO();
                    sameCount++;
                }
                else{
                    // If the current toy is not null
                    if(currentToy != slot.GetActiveToy().GetToyObjectSO()) {
                        return SortingType.NONE;
                    }
                    else{
                        sameCount++;
                    }
                }
            }
        }

        if(count == slotHolders.Count){
            return SortingType.BoxClear;
        }

        if(sameCount == slotHolders.Count){
            return SortingType.Match3;
        }
        else {
            return SortingType.NONE;
        }
    }

    public void TrySortingToy(){
        switch(IsSortable()){
            case SortingType.NONE:
                break;
            case SortingType.BoxClear:
                foreach(SlotHolder slot in slotHolders) {
                        slot.PushNewToyOut();
                    }
                break;
            case SortingType.Match3:
                if(sortable){
                    foreach(SlotHolder slot in slotHolders) {
                    slot.ClearSortedToys();
                    }
                    OnAnyToyShorted?.Invoke(this, EventArgs.Empty);
                }
                break;
            default:
                break;
        }
    }

    public int GetActiveToyNumber(){
        int count = 0;
        foreach(SlotHolder slot in slotHolders) {
            if(slot.HasActiveToy()) {
                count++;
            }
        }

        return count;
    }

}


public enum SortingType{
    BoxClear,
    Match3,
    NONE
}
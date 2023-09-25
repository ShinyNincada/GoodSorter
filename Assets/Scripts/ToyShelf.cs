using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyShelf : MonoBehaviour
{
    public static event EventHandler OnAnyToyShorted;
    [SerializeField] List<SlotHolder> slotHolders = new List<SlotHolder>(3);
    public SortingType IsSortable(){
        if(!slotHolders[0].HasActiveToy() && !slotHolders[1].HasActiveToy() && !slotHolders[2].HasActiveToy())
        {
            return SortingType.BoxClear;
        }
        
        if(!slotHolders[0].HasActiveToy() || !slotHolders[1].HasActiveToy() || !slotHolders[2].HasActiveToy()) {
            return SortingType.NONE;
        }

        if((slotHolders[0].GetActiveToy().GetToyObjectSO() == slotHolders[1].GetActiveToy().GetToyObjectSO()
        && slotHolders[1].GetActiveToy().GetToyObjectSO() == slotHolders[2].GetActiveToy().GetToyObjectSO()))
            return SortingType.Match3;
        
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
                foreach(SlotHolder slot in slotHolders) {
                    slot.ClearSortedToys();
                }
                OnAnyToyShorted?.Invoke(this, EventArgs.Empty);
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyShelf : MonoBehaviour
{
    public event EventHandler OnAnySortableToysExists;
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
                break;
            default:
                break;
        }
    
}

}


public enum SortingType{
    BoxClear,
    Match3,
    NONE
}
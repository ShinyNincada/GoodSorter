using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ToyShelf : MonoBehaviour
{
    [SerializeField] List<BaseSlot> slotHolders = new List<BaseSlot>(3);
    public bool canSort;

    public int GetActiveToyNumber(){
        int count = 0;
        foreach(SlotHolder slot in slotHolders) {
            if(slot.HasActiveToyObject()) {
                count++;
            }
        }

        return count;
    }

    public List<BaseSlot> GetSlotList()
    {
        return slotHolders;
    }

}


public enum SortingType{
    BoxClear,
    Match3,
    NONE
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blocked_Shelf : ToyShelf
{
    [SerializeField] private Image blockedImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] int blockValue = 3;
    bool isBlocking = true;

    void Start()
    {
        SortingManager.OnAnyToySorted += ToyShelf_OnAnyToySorted;
        text.text = $"{blockValue}X";
        if (blockValue > 0)
        {
            isBlocking = true;
        }
    }

    private void ToyShelf_OnAnyToySorted(object sender, EventArgs e)
    {
        blockValue--;
        if (blockValue <= 0)
        {
            isBlocking = false; 
            blockedImage.gameObject.SetActive(false);
            foreach (BaseSlot slot in GetSlotList())
            {
                slot.SetActiveToyObject(slot.GetActiveToyObject());
            }
        }
        else
        {
            text.text = $"{blockValue}X";
        }
    }

    public bool IsBlocking {
        get { return isBlocking; }
        set { isBlocking = value; }
    }

    private void OnDestroy()
    {
        SortingManager.OnAnyToySorted -= ToyShelf_OnAnyToySorted;
    }
}

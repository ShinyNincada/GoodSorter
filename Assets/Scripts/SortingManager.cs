using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;
using Unity.Mathematics;
using static ToyShelf;

public class SortingManager : MonoBehaviour
{
    public class OnAnyToySortedArgs : EventArgs
    {
        public Transform sortedTransform;

        public OnAnyToySortedArgs(Transform sortedTransform)
        {
            this.sortedTransform = sortedTransform;
        }
    }
    public static event EventHandler<OnAnyToySortedArgs> OnAnyToySorted;


    public static SortingManager Instance;
    public List<ToyObject> ToyObjectLists = new List<ToyObject>();
    public List<ToyObjectSO> waitingToyLists = new List<ToyObjectSO>();
    Dictionary<ToyObjectSO, int> toyObjectDictionary = new Dictionary<ToyObjectSO, int>();


    [SerializeField] private GameObject starPrefab;
    [SerializeField] private Transform targetTransform;

    [Space]
    [Header("Animation Settings")]
    [SerializeField] float speadRange;
    [SerializeField][Range(1.5f, 2f)] float minAnimDuration;
    [SerializeField][Range(2.5f, 4f)] float maxAnimDuration;

    [Space]
    [SerializeField] AudioSource CorrectSFX;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            
        }
        BaseSlot.OnActiveToyChanged += BaseSlot_OnActiveToyChanged;
    }

    private void BaseSlot_OnActiveToyChanged(object sender, BaseSlot.OnActiveToyChangedArgs e)
    {
        if(e.newToy == null)
        {
            RemoveToyObjectFromList(e.oldToy);
        }
        
        if(e.newToy != null)
        {
            AddToyObjectToList(e.newToy);
        }
    }

    private void ToyShelf_OnAnyToySorted(object sender, OnAnyToySortedArgs e)
    {
        CorrectSFX.Play();
        for (int i = 0; i < 3; i++) {
            StarSpawning_Tween(e.sortedTransform);
        }
    }


    

    public bool IsThereAnySortableActiveToys()
    {
        foreach(KeyValuePair<ToyObjectSO, int> keyValuePair in toyObjectDictionary)
        {
            if(keyValuePair.Value >= 3)
            {
                return true;
            }
        }

        return false;
        
    }

    public ToyObject[] GetRandomSortableToys()
    {
        List<ToyObject> result = new List<ToyObject>();
        foreach (KeyValuePair<ToyObjectSO, int> keyValuePair in toyObjectDictionary)
        {
            if (keyValuePair.Value >= 3 && result.Count == 0)
            {
                foreach(ToyObject toy in ToyObjectLists)
                {
                    if(toy.GetToyObjectSO() == keyValuePair.Key && result.Count < 3)
                    {
                        result.Add(toy);
                    }
                }
            }
        }

        if (result.Count == 3)
        {
            return result.ToArray();
        }
        else return null;
    }

    public void RemoveToyObjectFromList(ToyObject toy)
    {
        if(ToyObjectLists.Contains(toy))
        {
            ToyObjectLists.Remove(toy);
        }
        else
        {
            return;
        }
        if(toyObjectDictionary.ContainsKey(toy.GetToyObjectSO()))
        {
            toyObjectDictionary[toy.GetToyObjectSO()]--;
        }
        if (ToyObjectLists.Count == 0 && waitingToyLists.Count == 0)
        {
            GameManager.Instance.SetState(GameState.GameOver);
        }

    }

    public void AddToyObjectToList(ToyObject toy)
    {
        if (toy != null && !ToyObjectLists.Contains(toy))
        {
            ToyObjectLists.Add(toy);
        }
        else
        {
            return;
        }

        if (toyObjectDictionary.ContainsKey(toy.GetToyObjectSO()))
        {
            toyObjectDictionary[toy.GetToyObjectSO()]++;
        }
        else
        {
            toyObjectDictionary.Add(toy.GetToyObjectSO(), 1);
        }
    }

    public void RemoveRandomObject()
    {
        if (!IsThereAnySortableActiveToys())
        {
            Debug.Log("No sortable!!");
            return;
        }
        else
        {
            var arr = GetRandomSortableToys();
            if (arr != null) { 
                foreach(var item in arr)
                {
                    SlotHolder slot = item.transform.parent.parent.GetComponent<SlotHolder>();
                    slot.ClearPickupToy();
                }

            }
            
        }
    }

    void StarSpawning_Tween(Transform spawnPoint)
    {
        var spawnedStar = Instantiate(starPrefab, spawnPoint);
        spawnedStar.GetComponent<RectTransform>().anchoredPosition += new Vector2(Random.Range(-speadRange * 50, speadRange * 50), Random.Range(-speadRange * 50, speadRange * 50));
        float duration = Random.Range(minAnimDuration, maxAnimDuration);
        spawnedStar.transform.DOMove(targetTransform.position, duration)
            .SetEase(Ease.InOutBack)
            .OnComplete(() => {
                GameManager.Instance.AddStar(1);
                Destroy(spawnedStar);
            });
    }

    public SortingType IsSortable(ToyShelf shelf)
    {
        int count = 0;
        int sameCount = 0;
        ToyObjectSO currentToy = null;
        foreach (SlotHolder slot in shelf.GetSlotList())
        {
            if (!slot.HasActiveToyObject())
            {
                // If have no active toy
                if (currentToy != null)
                {
                    // if checked a toy in the shelf before
                    return SortingType.NONE;
                }
                else
                {
                    count++;
                }
            }
            else
            {
                // If slot has an active toy
                if (currentToy == null)
                {
                    //If current toy is null
                    currentToy = slot.GetActiveToyObject().GetToyObjectSO();
                    sameCount++;
                }
                else
                {
                    // If the current toy is not null
                    if (currentToy != slot.GetActiveToyObject().GetToyObjectSO())
                    {
                        return SortingType.NONE;
                    }
                    else
                    {
                        sameCount++;
                    }
                }
            }
        }

        if (count == shelf.GetSlotList().Count)
        {

            return SortingType.BoxClear;
        }

        if (sameCount == shelf.GetSlotList().Count)
        {

            return SortingType.Match3;
        }
        else
        {

            return SortingType.NONE;
        }
    }

    public void TrySortingToy(ToyShelf shelf)
    {
        switch (IsSortable(shelf))
        {
            case SortingType.NONE:
                break;
            case SortingType.BoxClear:
                foreach (SlotHolder slot in shelf.GetSlotList())
                {
                    slot.PushNewToyOut();
                }
                break;
            case SortingType.Match3:
                if (shelf.canSort && shelf.GetActiveToyNumber() == 3)
                {
                    foreach (SlotHolder slot in shelf.GetSlotList())
                    {
                        slot.ClearSortedToys();
                    }
                    
                    OnAnyToySorted?.Invoke(this, new OnAnyToySortedArgs(this.transform));
                }
                break;
            default:
                break;
        }
    }


    private void OnDestroy()
    {
        BaseSlot.OnActiveToyChanged -= BaseSlot_OnActiveToyChanged;
    }
}

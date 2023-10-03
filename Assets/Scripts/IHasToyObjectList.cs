using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IHasToyObjectList
{
    public void AddNewToy(ToyObject newToy);

    public void RemoveFirstToyFromList();

    public ToyObject GetFirstToy();

    public ToyObject GetSecondToy();

    public List<ToyObject> GetToyList();
}

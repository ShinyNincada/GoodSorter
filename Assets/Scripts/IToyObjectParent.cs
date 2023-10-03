using UnityEngine;

public interface IToyObjectParent
{
    
    public Transform GetToyObjectHolder();
    public ToyObject GetActiveToyObject();
    public void SetActiveToyObject(ToyObject obj);
    public bool HasActiveToyObject();
    
}
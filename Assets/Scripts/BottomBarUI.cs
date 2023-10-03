using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarUI : MonoBehaviour
{
    [SerializeField] private Button HammerButton;

    // Start is called before the first frame update
    void Start()
    {
        HammerButton.onClick.AddListener(() =>
        {
            SortingManager.Instance.RemoveRandomObject();
        });  
    }

}

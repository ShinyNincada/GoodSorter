using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button PlayAgainButton;
    [SerializeField] Button HomeButton;

    // Start is called before the first frame update
    void Start()
    {
        PlayAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Scene.GameScene);
        });

        HomeButton.onClick.AddListener(() => { 
            Debug.Log("Test Home Button");
        }); 
    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComboTextUI : MonoBehaviour
{
    float comboTimer;
    float comboTimerMax = 10;
    [SerializeField] TMP_Text text;
    public Image _fillImage;
    int count = 0;
   private void Start() {
        ToyShelf.OnAnyToyShorted += ToyShelf_OnAnyToyShorted;
   }

    private void Update() {
        comboTimer -= Time.deltaTime;

        _fillImage.fillAmount = comboTimer/comboTimerMax;
    }


    private void ToyShelf_OnAnyToyShorted(object sender, EventArgs e)
    {
        
        if(comboTimer > 0) {
            count++;
            Debug.Log("Combooo");
        }
        else{
            count = 1;
        }
        
        comboTimer = comboTimerMax;
        text.text = count.ToString();
    }
}

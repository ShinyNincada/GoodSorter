using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ComboTextUI : MonoBehaviour
{
    [SerializeField] private GameObject ComboTextPrefab;
    [SerializeField] TMP_Text comboText;

    float comboTimer;
    float comboTimerMax = 10;
    public Image _fillImage;
    int count = 0;
    
   private void Start() {
        SortingManager.OnAnyToySorted += SortingManager_OnAnyToyShorted;
   }

    private void Update() {
        comboTimer -= Time.deltaTime;

        _fillImage.fillAmount = comboTimer/comboTimerMax;
    }


    private void SortingManager_OnAnyToyShorted(object sender, SortingManager.OnAnyToySortedArgs e)
    {
        if (comboTimer > 0) {
            count++;
        }
        else {
            count = 1;
        }

        comboTimer = comboTimerMax;
        comboText.transform.DOScale(1.3f, 1f).SetEase(Ease.OutElastic)
            .OnComplete(() => { comboText.transform.localScale = Vector3.one; 
        });;

        SpawnComboText(e.sortedTransform, count);
        comboText.text = $"Combo: {count}";
    }

    public void SpawnComboText(Transform shelfTransform, int combo)
    {
        var spawnedText = Instantiate(ComboTextPrefab, shelfTransform);
        COMBO value;
        if (combo < 5)
        {
           value  = (COMBO)Enum.GetValues(typeof(COMBO)).GetValue(combo);
        }
        else
        {
            value = COMBO.COMBO;
        }

        spawnedText.GetComponent<TMP_Text>().text = value.ToString();
        spawnedText.transform.DOScale(1.3f, 0.5f).SetEase(Ease.OutElastic)
            .OnComplete(() => { Destroy(spawnedText.gameObject); });

    }

    private void OnDestroy()
    {
        SortingManager.OnAnyToySorted -= SortingManager_OnAnyToyShorted;
    }
}


public enum COMBO
{
    COOL = 1,
    GREAT,
    EXCELLENT,
    PERFECT,
    COMBO
}
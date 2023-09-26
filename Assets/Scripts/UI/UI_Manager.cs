using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text starCountText;

    private void Update() {
        timerText.text = GameManager.Instance.GetTimerText();
        starCountText.text = GameManager.Instance.GetStarCount().ToString();
    }
}

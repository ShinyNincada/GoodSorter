using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text starCountText;

    [Space]
    [SerializeField] Transform GameOverUI;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        switch(GameManager.Instance.GetState())
        {
            case GameState.Playing:
                GameOverUI.gameObject.SetActive(false);
                break;
            case GameState.GameOver:
                GameOverUI.gameObject.SetActive(true);
                break;
            default:
                break;
               
        }
    }

    private void Update() {
        timerText.text = GameManager.Instance.GetTimerText();
        starCountText.text = GameManager.Instance.GetStarCount().ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }
}

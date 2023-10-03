using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnGameStateChanged;
    public event EventHandler OnStarCountChanged;
    public static GameManager Instance;
    public float playTimer;
    public int starCount = 0;
    [SerializeField] GameState state;


    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            
        }
        SetState(GameState.Playing);
    }


    // Update is called once per frame
    void Update()
    {
        PlayTimerHandle();
       
    }

    private void PlayTimerHandle()
    {
        if(playTimer > 0) {
            playTimer -= Time.deltaTime;
        }
        else if (playTimer <= 0) {
            SetState(GameState.GameOver);
            playTimer = 0;
        }
    }

    public void SetState(GameState newState)
    {
        state = newState;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public GameState GetState()
    {
        return state;
    }

    public string GetTimerText(){
        int minutes = Mathf.FloorToInt(playTimer / 60 );
        int seconds = Mathf.FloorToInt(playTimer % 60 );
        return $"{minutes}:{seconds}";
    }

    public void AddStar(int amount) {  
        starCount += amount;
        
        OnStarCountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetStarCount(){
        return starCount;
    }
}

public enum GameState {
    GameOver,
    Playing,
    Pause
}

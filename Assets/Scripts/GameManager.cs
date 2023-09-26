using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float playTimer;
    public int starCount = 0;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
        else if (playTimer < 0) {
            playTimer = 0;
        }
        
    }

    public string GetTimerText(){
        int minutes = Mathf.FloorToInt(playTimer / 60 );
        int seconds = Mathf.FloorToInt(playTimer % 60 );
        return $"{minutes}:{seconds}";
    }

    public void GetStars(int amount) {
        starCount += amount;
    }

    public int GetStarCount(){
        return starCount;
    }
}

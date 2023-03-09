using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public PlayerController Player;
    [SerializeField] private Camera cameraObject;

    [SerializeField] private float ingameTime;
    [SerializeField] private float baseGameStageTime = 10f;
    [SerializeField] private float gameStageInt = 0f;

    public Action NextStageEvent;

    private static GameManagerController _instance;
    public static GameManagerController Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No GameManagerInstance found!");

            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;
    }

    public bool _isGameOver = false;

    public void GameOver()
    {
        _isGameOver = true;
    }

    public PlayerController getPlayer()
    {
        return Player;
    }

    public void SetPlayer(PlayerController player)
    {
        Player = player;
        
    }

    public Camera GetCamera()
    {
        return cameraObject;
    }

    

    void Update()
    {
        //GameTimer
        ingameTime += Time.deltaTime;
        if(ingameTime > gameStageInt)
        {
            gameStageInt += baseGameStageTime;
            NextStageEvent.Invoke();
        }
    }

    private void RestartTimer()
    {
        ingameTime = 0;
    }

    
}

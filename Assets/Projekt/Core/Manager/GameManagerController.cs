using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public PlayerController Player;
    [SerializeField] private Camera cameraObject;

    [SerializeField] private float ingameTime;
    [SerializeField] private float baseGameStageTime = 10f;
    [SerializeField] private float gameStageInt = 10f;

    public Action NextStageEvent;

    private Coroutine coroutine;

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
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }

    public void StartGame() 
    {
        Time.timeScale = 1.0f;
        Instance.StartGameLevel();
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

    public void StartGameLevel()
    {
        RestartTimer();
        coroutine = StartCoroutine(IngameTimer());
    }
    
    public void PauseGameLevel()
    {
        StopCoroutine(coroutine);
    }

    public void ResumeGameLevel()
    {
         coroutine = StartCoroutine(IngameTimer());
    }


    void Update()
    {
        
    }

    private IEnumerator IngameTimer()
    {
        while(true)
        {

            //GameTimer
            ingameTime += Time.deltaTime;
            if (ingameTime > gameStageInt)
            {
                gameStageInt += baseGameStageTime;
                NextStageEvent.Invoke();
            }
            yield return new WaitForEndOfFrame();

        }
    }


    //Should be Called when the Game Stage restarts
    private void RestartTimer()
    {
        ingameTime = 0;
        gameStageInt = baseGameStageTime;
    }

    
}

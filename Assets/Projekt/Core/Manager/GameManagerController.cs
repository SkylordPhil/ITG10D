using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public PlayerController Player;

    private static GameManagerController _instance;
    public static GameManagerController Instance
    {
        get
        {
            if (_instance = null)
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
       
}

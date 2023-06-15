using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    
    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync("Enemy Test");
        SceneManager.UnloadSceneAsync("GameOverScene");

        LevelManager.instance.UnloadCurrentLevel();
        LevelManager.instance.LoadLevel(0);
        GameManagerController.Instance.StartGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.UnloadSceneAsync("Enemy Test");
        SceneManager.UnloadSceneAsync("GameOverScene");
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
    }

}

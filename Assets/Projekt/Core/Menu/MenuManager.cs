using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Helper;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject controlsMenu;


    #region MainMenu
    public void StartGame()
    {
        LevelManager.instance.LoadLevel(0);
        SceneManager.UnloadSceneAsync("MainMenuScene");
        //weiter zu gameManager -> Spiel-Szene laden
        CameraController.Instance.isIngame = true;
        GameManagerController.Instance.StartGameLevel();
        Time.timeScale = 1;
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("IngameMenuScene");
        CameraController.Instance.isIngame = true;
        LevelManager.instance.UnloadMenu();
    }

    public void ReturnToMenu()
    {
        
        LevelManager.instance.UnloadCurrentLevel();
        SceneManager.UnloadSceneAsync("IngameMenuScene");
        SceneManager.LoadSceneAsync("MainMenuScene", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ShowCredits() 
    { 
        //placeholder
    }
    #endregion

    #region SettingsMenu
    public void OpenAudioMenu()
    {
        settingsMenu.SetActive(false);
        audioMenu.SetActive(true);
    }

    public void OpenControlsMenu()
    {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    #endregion

    #region AudioMenu
    public void CloseAudioMenu()
    {
        audioMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ResetAudioSettings()
    {

    }

    #endregion

    #region ControlsMenu
    public void CloseControlsMenu()
    {
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    #endregion

    public void DetermineMenuType(MenuType menuType, float currentValue, string stringParam)
    {
        //SetExposedParam
        switch(menuType)
        {
            case MenuType.AudioMenu:
                AudioManager.GetInstance().SetExposedParam(stringParam, currentValue);
                break;

        }
    }
}
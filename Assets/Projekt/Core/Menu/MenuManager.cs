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
    [SerializeField] private GameObject credits;


    #region MainMenu
    public void StartGame()
    {
        SceneManager.UnloadSceneAsync("MainMenuScene");
        LevelManager.instance.LoadLevel(0);
        //weiter zu gameManager -> Spiel-Szene laden
        CameraController.Instance.isIngame = true;
        GameManagerController.Instance.StartGame();
    }

    public void BackToMenu()
    {
        SceneManager.UnloadSceneAsync("Enemy Test");
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

    #region Credits

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
    }

    #endregion

    public void DetermineMenuType(MenuType menuType, float currentValue, string sliderName)
    {
        //SetExposedParam
        switch(menuType)
        {
            case MenuType.AudioMenu:
                AudioManager.GetInstance().SetExposedParam(sliderName, currentValue);
                break;
        }
    }
}
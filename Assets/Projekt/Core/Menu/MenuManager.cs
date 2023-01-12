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


    #region MainMenu
    public void StartGame()
    {
        //weiter zu gameManager -> Spiel-Szene laden
    }


    public void QuitGame()
    {
        /* 
         * Weiter zu GameManager
        Application.Quit();
        Debug.Log("Quit Pressed");
        
        #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
        #endif
        */
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
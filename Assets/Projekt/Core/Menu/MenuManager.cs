using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;

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
        this.gameObject.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ShowCredits() 
    { 
        //placeholder
    }
}
  
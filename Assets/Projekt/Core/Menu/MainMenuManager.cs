using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame() => SceneManager.LoadScene("DebugScene");

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Pressed");
        
        #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

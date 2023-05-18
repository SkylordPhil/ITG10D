using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manager that Loads and Unloads the current Selected Game Level
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    [SerializeField] private Scene mainMenuScene;
    [SerializeField] private Scene inGameMenuScene;


    [SerializeField] private GameLevel currentLevel;

    
    [SerializeField] private GameLevel[] allLevels;


    private void Awake()
    {
        if(_instance != null)
        {
            Debug.Log("This LevelManager Was 1 too many.... Selfdestroying.....");
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }



    
    public static LevelManager instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("No LevelManager instatiated..... maybe persistenScene is missing?");
            }
            return _instance;
        }
    }

    public void LoadMenu()
    {
        
    }

    public void UnloadMenu()
    {

    }
    

    /// <summary>
    /// Load a Level Scene and set it as the current GameLevel.
    /// </summary>
    /// <param name="levelNumber">
    /// Index in the allLevels Array
    /// </param>
    public void LoadLevel(int levelNumber)
    {

        SceneManager.LoadScene(allLevels[levelNumber].levelPath, LoadSceneMode.Additive);
        currentLevel = allLevels[levelNumber];

    }


    /// <summary>
    /// Unload the Current Level and change the currentLevel to null
    /// </summary>
    public void UnloadCurrentLevel()
    {
        SceneManager.UnloadSceneAsync(currentLevel.levelPath);
        currentLevel = null;

    }

    [ContextMenu("Debug Load")]
    void DebugLoad()
    {
        LoadLevel(0);
        
    }

    
}

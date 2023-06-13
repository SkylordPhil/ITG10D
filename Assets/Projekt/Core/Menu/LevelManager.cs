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

    private bool _isLoading;


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
        //prevents LoadSceneAsync from loading two or more ingameMenuScene
        if(_isLoading)
        {
            return;
        }
        //makes camera stop following the player
        CameraController.Instance.isIngame = false;
        GameManagerController.Instance.Player.gameObject.SetActive(false);
        //pauses the update logic
        Time.timeScale = 0;
        //loads the IngameMenuScene
        var op = SceneManager.LoadSceneAsync("IngameMenuScene", LoadSceneMode.Additive);
        _isLoading = true;
        StartCoroutine(LoadMenuRoutine(op));

    }

    public void UnloadMenu()
    {
        CameraController.Instance.isIngame = true;
        GameManagerController.Instance.Player.gameObject.SetActive(true);
        //pauses the update logic
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("IngameMenuScene");
    }
    

    /// <summary>
    /// Load a Level Scene and set it as the current GameLevel.
    /// </summary>
    /// <param name="levelNumber">
    /// Index in the allLevels Array
    /// </param>
    public void LoadLevel(int levelNumber)
    {

        //SceneManager.LoadScene(allLevels[levelNumber].levelPath, LoadSceneMode.Additive);
        var load = SceneManager.LoadSceneAsync(allLevels[levelNumber].levelPath, LoadSceneMode.Additive);
        currentLevel = allLevels[levelNumber];
        StartCoroutine(LoadLevelRoutine(load));

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


    IEnumerator LoadLevelRoutine(AsyncOperation op)
    {
        while(!op.isDone)
        {
            //
            yield return new WaitForEndOfFrame();
        }
        //sets EnemyTestScene to active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(currentLevel.levelPath));
    }

    IEnumerator LoadMenuRoutine(AsyncOperation op)
    {
        
        while(!op.isDone)
        {
            //executed while loading the IngameMenuScene hasn't been finished
            yield return new WaitForEndOfFrame();
        }
        _isLoading = false;
    }
    
}

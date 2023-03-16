using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject Boss;
    public int gameStage = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTicks());

        GameManagerController.Instance.NextStageEvent -= StageUpdate;
        GameManagerController.Instance.NextStageEvent += StageUpdate;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("This EnemySpawner Was 1 too many.... Selfdestroying.....");
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    public static EnemySpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("No LevelManager instatiated..... maybe persistenScene is missing?");
            }
            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    IEnumerator SpawnTicks()
    {
        while (true)
        {
            if (gameStage == 1)
            {
                yield return new WaitForSeconds(5f);
                gameStage++;
            }
            else
            {
                yield return new WaitForSeconds(30f);
            }

            Vector3 playerPosition = GameManagerController.Instance.Player.transform.position;

            int entityNumber = EnemyNumber(gameStage);
            //gameStage ^ 2

            for (int i = 0; i <= entityNumber; i++)
            {
                Vector3 spawnPosition = playerPosition + (Vector3)Random.insideUnitCircle.normalized * 15;
                Instantiate(Enemy, spawnPosition, Quaternion.Euler(0, 0, 0));
            }
        }
    }

    private int EnemyNumber(int p_gameStage)
    {
        int startnumber = 10;
        int enemynumber;

        enemynumber = startnumber + (p_gameStage * 5);

        return enemynumber;
    }


    private void StageUpdate()
    {
        gameStage++;
    }
}

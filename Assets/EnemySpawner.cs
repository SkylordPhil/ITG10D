using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject RangedEnemy;
    [SerializeField] private GameObject Boss;
    public int gameStage = 0;
    
    private int groupnumber;
    private float wavePause = 5f;
    private float gameStageLength = 30f;

    [SerializeField] int baseMaxEnemieHp = 10;
    public int currentEnemieHp;
    [SerializeField] private int hpIncrease = 2;

    // Start is called before the first frame update
    void Start()
    {
        baseMaxEnemieHp = Enemy.GetComponent<BaseEnemy>().maxHealth;
        currentEnemieHp = baseMaxEnemieHp;
        StartCoroutine(SpawnTicks());

        /*GameManagerController.Instance.NextStageEvent -= StageUpdate;
        GameManagerController.Instance.NextStageEvent += StageUpdate;*/
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
                //StageUpdate();
                yield return new WaitForSeconds(wavePause/2);
            }

            Vector3 playerPosition = GameManagerController.Instance.Player.transform.position;

            int entityNumber = GroupNumber(gameStage);

            for (int i = 0; i <= entityNumber; i++)
            {
                Vector3 spawnCircle = playerPosition + (Vector3)Random.insideUnitCircle.normalized * 20;

                int maxAmount = 4;
                int randEnemyNumb = Random.Range(1 + (gameStage/2), maxAmount + Random.Range(0, gameStage));
                for (int x = 0; x <= randEnemyNumb; x++)
                {
                    Vector3 spawnPosition = spawnCircle + (Vector3)Random.insideUnitCircle.normalized * 2;
                    Instantiate(Enemy, spawnPosition, Quaternion.Euler(0, 0, 0));
                }

                float timeBetweenGroup = gameStageLength / entityNumber;
                float pause = Random.Range(0, timeBetweenGroup * 2);
                yield return new WaitForSeconds(pause);
            }

            StageUpdate();
            HpUpdate();
            yield return new WaitForSeconds(wavePause);
            
        }
    }

    private int GroupNumber(int p_gameStage)
    {
        int startnumberGroupes = 3;

        groupnumber = startnumberGroupes + ((p_gameStage -1) * 3);

        //Debug.Log("GameStage: " + gameStage);
        //Debug.Log("Groupes: " + groupnumber);
        return groupnumber;
    }

    private void HpUpdate()
    {
        Debug.Log("Enemies grow stronger.");
        currentEnemieHp = baseMaxEnemieHp + hpIncrease;
    }

    private void StageUpdate()
    {
        gameStage++;
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public float currentSpeed;
    public int maxHealth = 10;
    public int currentHealth;
    public int damage = 1;
    private int splinterCount = 0;

    private bool slowed = false;
    public bool frozen = false;
    private bool fire = false;
    private float fireTick = 1;
    private float currentTick;

    private float duration_slowed;
    private float duration_frozen;
    private float duration_fire;

    private float fireDmg;

    private Vector3 lastPostition;

    [SerializeField] private GameObject xpPrefab;
    [SerializeField] private GameObject baseBullet;
    [SerializeField] private GameObject MagmaIstance;

    public PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = speed;
        currentTick = fireTick;
        Player = GameManagerController.Instance.getPlayer();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Transform Playerposition = Player.transform;

        Vector3 displacement = Playerposition.position - transform.position;
        displacement = displacement.normalized;

        if (Vector2.Distance(Playerposition.position, transform.position) > 1.0f)
        {
            transform.position += (displacement * currentSpeed * Time.deltaTime);
        }

        if (slowed || frozen || fire)
        {
            Timer();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            Cold();
            Ice();
            Magma();
            bool burning = Player.GetComponent<PlayerController>().fireDmg;
            if (burning)
            {
                Burning();
            }
        }
    }

    // Function for death
    public void Death()
    {            
        lastPostition = transform.position;

        GameObject xpOrbPrefab = xpPrefab;
        Instantiate(xpOrbPrefab, lastPostition, Quaternion.identity);

        bool splinter = Player.GetComponent<PlayerController>().splinters;
        if (splinter && splinterCount == 0)
        {
            Splinter();
            splinterCount = 1;
        }

        Destroy(gameObject);
    }

    private void Cold()
    {
        bool cold = Player.GetComponent<PlayerController>().cold;
        if (cold)
        {
            Debug.Log("Freezing");

            duration_slowed = Player.GetComponent<PlayerController>().coldTime;
            float slowedBy = Player.GetComponent<PlayerController>().coldEffect;

            currentSpeed = currentSpeed * (1 - slowedBy);
            slowed = true;
        }
    }

    private void Ice()
    {
        bool ice = Player.GetComponent<PlayerController>().ice;
        if (ice)
        {
            Debug.Log("Iceing over");
            currentSpeed = 0;

            float frozen_min = Player.GetComponent<PlayerController>().iceMinTime;
            float frozen_max = Player.GetComponent<PlayerController>().iceMaxTime;
            duration_frozen = Random.Range(frozen_min, frozen_max);

            frozen = true;
        }
    }

    public void Burning()
    {
        fireDmg = Player.GetComponent<PlayerController>().fireDmgAmount;
        duration_fire = Player.GetComponent<PlayerController>().fireTime;
        fire = true;
    }

    private void Magma()
    {
        bool magma = Player.GetComponent<PlayerController>().firefountain;
        if (magma)
        {
            int rand = Random.Range(1, 11);
            if (rand == 1)
            {
                GameObject currentMagma = Instantiate(MagmaIstance);
                currentMagma.transform.position = transform.position;
            }
        }
    }

    private void Splinter()
    {
        Debug.Log("Splintering");
        
        int amount = Player.GetComponent<PlayerController>().splintersAnz;
        for (int i = 0; i < amount; i++)
        {
            Vector3 attackDirection = (Vector3)Random.insideUnitCircle.normalized;

            GameObject currentBullet = Instantiate(baseBullet);
            currentBullet.transform.position = transform.position;
            currentBullet.GetComponent<Bullet>().SetMoveInfo(attackDirection);
        }
    }

    private void Timer()
    {
        if (slowed)
        {
            duration_slowed -= Time.deltaTime;
            if (duration_slowed <= 0)
            {
                currentSpeed = speed;
                slowed = false;
            }
        }

        if (frozen)
        {
            duration_frozen -= Time.deltaTime;
            if (duration_frozen <= 0)
            {
                currentSpeed = speed;
                frozen = false;
            }
        }

        if (fire)
        {
            currentTick -= Time.deltaTime;
            duration_fire -= Time.deltaTime;
            if (currentTick <= 0)
            {
                Debug.Log("Brennt");
                TakeDamage((int)fireDmg);
                currentTick = fireTick;
            }
            if (duration_fire <= 0)
            {
                fire = false;
            }
        }
        
    }
}
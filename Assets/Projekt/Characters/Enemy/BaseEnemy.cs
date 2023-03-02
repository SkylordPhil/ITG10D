using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public int maxHealth = 1;
    public int currentHealth;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        Player = GameManagerController.Instance.getPlayer().gameObject;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Transform Playerposition = Player.transform;

        Vector3 displacement = Playerposition.position - transform.position;
        displacement = displacement.normalized;

        if (Vector2.Distance(Playerposition.position, transform.position) > 1.0f)
        {
            transform.position += (displacement * speed * Time.deltaTime);
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

    // Function for death
    void Death()
    {
        Destroy(gameObject);
    }
}
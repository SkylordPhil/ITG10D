using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {

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
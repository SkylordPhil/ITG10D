using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirefountainScript : MonoBehaviour
{
    public float TTL = 20;
    public float currentTTL;
    private float cooldown = 1;
    private float time;
    public int playerDmg = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentTTL = TTL;
        time = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        TTL -= Time.deltaTime;
        if (TTL <= 0)
        {
            Destroy(gameObject);
        }

        time -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (time <= 0)
            {
                collision.gameObject.GetComponent<BaseEnemy>().Burning();
                time = cooldown;
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(playerDmg);
        }
    }

    public void TTL_Up(int increase)
    {
        currentTTL += increase;
    }
}

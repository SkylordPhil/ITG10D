using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExperiencePoint : MonoBehaviour
{
    public int XP;
    public float speed = 2f;
    public float range = 3f;
    //public PlayerController Player;
    private GameObject Player;
    private GameObject Raven;
    

    public bool ravenSpecial = false;

    // Start is called before the first frame update
    void Start()
    {
        //Player = GameManagerController.Instance.getPlayer();
        Player = GameObject.FindGameObjectWithTag("Player");
        Raven = GameObject.FindGameObjectWithTag("Raven");
    }

    // Update is called once per frame
    void Update()
    {
        float distancePlayer = Vector2.Distance(transform.position, Player.transform.position);
        if (distancePlayer <= range)
        {
            Attracted(Player);
        }

        if (Raven != null && !ravenSpecial)
        {
            float distanceRaven = Vector2.Distance(transform.position, Raven.transform.position);
            bool pickup = Raven.GetComponent<RavenBase>().pickup;

            if (distanceRaven <= range && pickup)
            {
                Attracted(Raven);
            }
        }
    }

    //mit dem Collider will es gerade noch nicht funktionieren
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            OnReachedPlayer();
        }
        
        if (collider.gameObject.CompareTag("Raven"))
        {
            Debug.Log("Raben-XP");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Raven"))
        {
            Debug.Log("Raben-XP");
            Destroy(gameObject);
        }
    }

    private void OnReachedPlayer()
    {
        //GameManagerController.Instance.Player.GetXP(10);
        Player.GetComponent<PlayerController>().GetXP(XP);
    }
    
    private void Attracted(GameObject target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);  
    }

    public void SpawnXP()
    {

    }

    public void ModifyRange(float mod)
    {
        range = mod;
    }

    public void ModifySpeed(float mod)
    {
        speed = mod;
    }
}

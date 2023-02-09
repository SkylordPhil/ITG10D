using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, IDamageable
{
    public float speed = 2f;
    public GameObject Player;
    public float healthAmount = 100;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameManagerController.Instance.getPlayer().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Transform Playerposition = Player.transform;

        Vector3 displacement = Playerposition.position - transform.position;
        displacement = displacement.normalized;

        if (Vector2.Distance(Playerposition.position, transform.position) > 1.0f)
        {
            transform.position += (displacement * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2d(Collision2D coll)
    {

    }

    public void TakeDamage(int dmg)
    {
        healthAmount -= dmg;
    }
}

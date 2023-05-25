using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExperiencePoint : MonoBehaviour
{
    public int XP = 1;
    public float speed = 4f;
    public float range = 3f;
    public PlayerController Player;

    private bool attracted = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameManagerController.Instance.getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance <= range)
        {
            attracted = true;
        }

        if (attracted)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
        }
    }

    //mit dem Collider will es gerade noch nicht funktionieren
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnReachedPlayer()
    {
        GameManagerController.Instance.Player.GetXP(1);
    }

    public void SpawnXP()
    {

    }
}

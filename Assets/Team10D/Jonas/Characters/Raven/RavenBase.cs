using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class RavenBase : MonoBehaviour
{
    private float speed = 5f;
    private float speed_increase;
    public float speed_current;
    public int inventory_limit = 15;
    private int inventory_current;
    public int inventory;
    private bool returnHome = false;
    public bool pickup = true;

    [SerializeField] private GameObject xpPrefab;
    private GameObject Player;
    private GameObject XpOrb;

    Rigidbody RavenRigBody;

    // Start is called before the first frame update
    void Start()
    {
        XpOrb = GameObject.FindGameObjectWithTag("XP");
        Player = GameObject.FindGameObjectWithTag("Player");

        speed_current = speed;
    }

    // Update is called once per frame
    void Update()
    {
        XpOrb = GameObject.FindGameObjectWithTag("XP");
        Player = GameObject.FindGameObjectWithTag("Player");

        //CollectXP();
        //ReturnToPlayer();

        if (returnHome == false && XpOrb != null)
        {
            CollectXP();
        }
        else if (returnHome == true || XpOrb == null)
        {
            ReturnToPlayer();
        }
        else
        {
            IdleState();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "XP")
        {
            inventory += 1;
            InventoryCheck();
        }

        if (collision.gameObject.tag == "Player" && inventory == inventory_current)
        {
            EmptyInventory();
        }
    }

    private void Move(GameObject destination)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination.transform.position, speed_current * Time.deltaTime);
        //RavenRigBody.MovePosition(destination.transform.position /*+ speed_current * Time.deltaTime*/);
    }

    private void CollectXP()
    {
        //Transform XpPosition = XpOrb.transform;
        //Move(XpPosition);
        Move(XpOrb);
    }

    private void ReturnToPlayer()
    {
        //Transform Playerposition = Player.transform;
        //Move(Playerposition);
        Move(Player);
    }

    private void IdleState()
    {
        if (inventory != 0)
        {
            ReturnToPlayer();
        }
        else
        {
            //Implement Idle Movement instead of ReturnToPlayer
            ReturnToPlayer();
        }
    }
    
    private void InventoryCheck()
    {
        if (inventory == inventory_current)
        {
            returnHome = true;
            pickup = false;
            gameObject.tag = "RavenFull";
        }
    }

    private void EmptyInventory()
    {
        Debug.Log("Rabe stößt an Spieler");
        GameObject xpOrbPrefab = xpPrefab;
        Vector2 lastPostition = transform.position;

        for (int i = inventory; i >= 0; i--)
        {
            Instantiate(xpOrbPrefab, lastPostition, Quaternion.identity);
            inventory--;
        }

        inventory = 0;
        returnHome = false;
        gameObject.tag = "Raven";
    }

    public void InventoryUp(int increase)
    {
        inventory_current = inventory_limit + increase;
    }

    public void SpeedUp(float increase)
    {
        speed_increase += increase;
        speed_current = speed + speed_increase;
    }

    public void ResetValues()
    {
        //Has to be called when the game Ends
        inventory_current = inventory_limit;
        speed_increase = 0;
    }
}

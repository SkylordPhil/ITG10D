using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class RavenBase : MonoBehaviour
{
    public float speed_current;

    public int inventory_current;
    public int inventory;

    private bool returnHome = false;
    public bool pickup;

    [SerializeField] private GameObject xpPrefab;
    private GameObject Player;
    private PlayerController PlayerContr;
    private GameObject[] XpOrb;
    private GameObject TargetOrb;

    private float idleTimer;
    public float idleWidth = 1;

    // Start is called before the first frame update
    void Start()
    {
        XpOrb = GameObject.FindGameObjectsWithTag("XP");
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerContr = GameManagerController.Instance.getPlayer();

        speed_current = PlayerContr.ravenSpeedCurrent;
        inventory_current = PlayerContr.ravenInventoryLimitCurrent;

        idleTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        XpOrb = GameObject.FindGameObjectsWithTag("XP");
        Player = GameObject.FindGameObjectWithTag("Player");

        speed_current = PlayerContr.ravenSpeedCurrent;
        inventory_current = PlayerContr.ravenInventoryLimitCurrent;

        if (returnHome == false && (XpOrb.Length != 0 || TargetOrb != null))
        {
            CollectXP();
            InventoryCheck();
        }
        else if(returnHome == true || XpOrb.Length == 0)
        {
            ReturnToPlayer();

            Vector2 diff = transform.position - Player.transform.position;
            Vector2 ak_diff = new Vector2(0.05f, 0.05f);
            if (diff.x <= ak_diff.x && diff.y <= ak_diff.y && inventory > 0)
            {
                EmptyInventory();
            }
        }
        else
        {
            ReturnToPlayer();
        }
    }

    private void Move(GameObject destinationObject)
    { 
        transform.position = Vector2.MoveTowards(transform.position, destinationObject.transform.position, speed_current * Time.deltaTime);
    }

    private void CollectXP()
    {
       if (TargetOrb == null)
       {
            int random = Random.Range(0, XpOrb.Length);
            XpOrb[random].tag = "TargetedXp";
            TargetOrb = GameObject.FindGameObjectWithTag("TargetedXp");
        }
       else
       {
           Move(TargetOrb);
           XpOrb = GameObject.FindGameObjectsWithTag("XP");
        }
    }

    private void ReturnToPlayer()
    {
        Move(Player);
    }

    private void IdleMovement()
    {
        Vector2 center = Player.transform.position;
        float radius = 2.0f;

        float angle =+ speed_current * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = center + offset;
    }

    public void EmptyInventory()
    {
        int xpValue = xpPrefab.GetComponent<ExperiencePoint>().XP;
        Vector2 lastPostition = transform.position;

        for (int i = inventory; i >= 0; i--)
        {
            Player.GetComponent<PlayerController>().GetXP(xpValue);
            inventory--;
        }

        inventory = 0;
        returnHome = false;
        gameObject.tag = "Raven";
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

    public void addXP()
    {
        inventory++;
        InventoryCheck();
    }

    public void EnablePickup()
    {
        pickup = true;
    }
}

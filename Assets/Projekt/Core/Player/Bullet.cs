using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float currentMoveSpeed;
    private float moveSpeed = 10f;
    

    [SerializeField] public int currentDamage;
    //private int baseDamage = 3;

    [SerializeField] private float lifeTime = 1f;
    private float currentLifeTime = 0;

    [SerializeField] private int currentPenetrationAmount;
    private int penetrationAmount = 0;

    private float lifeTimeIncrease = 0;

    private Vector2 moveDirection;

    private int penetrationAmountIncrease = 0;
    //private int damageIncrease = 0;
    private int speedIncrease = 0;

    public PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        //Find Way to Reset Damage and Penetration Increases
        //currentDamage = baseDamage;
        currentMoveSpeed = moveSpeed;
        currentPenetrationAmount = penetrationAmount;

        Player = GameManagerController.Instance.getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        currentDamage = Player.currentDamage;
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime > lifeTime)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(moveDirection * currentMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            //currentDamage = baseDamage + damageIncrease;
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(currentDamage);

            
            if (currentPenetrationAmount == 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                currentPenetrationAmount -= 1;
            }
            
        }
    }

    public void SetMoveInfo(Vector2 dir)
    {
        transform.rotation = Quaternion.Euler(dir.x, dir.y, 0f);
        moveDirection = dir;
    }

    /*public void UpDamage(int increase)
    {
        damageIncrease += increase;
        currentDamage += damageIncrease;
        Debug.Log("Bullet Damage: " + currentDamage);
    }*/

    public void UpSpeed(float increase)
    {
        speedIncrease += (int)increase;
        currentMoveSpeed += speedIncrease;
    }

    public void UpPenetration(int increase)
    {
        penetrationAmountIncrease += increase;
        currentPenetrationAmount += penetrationAmountIncrease;
    }

    public int GetDamage() { return currentDamage; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private float speedIncrease;
    [SerializeField] private int baseDamage = 3;
    [SerializeField] public int currentDamage;
    [SerializeField] private int damageIncrease = 0;
    [SerializeField] private float lifeTime = 2f;


    private float currentLifeTime = 0;
    [SerializeField] private int penetrationAmount = 0;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        //Find Way to Reset Damage and Penetration Increases
    }

    // Update is called once per frame
    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime > lifeTime)
        {
            Destroy(this.gameObject);
        }

        float currentMoveSpeed = moveSpeed + speedIncrease;
        transform.Translate(moveDirection * currentMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            currentDamage = baseDamage + damageIncrease;
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(currentDamage);

            
            if (penetrationAmount == 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                penetrationAmount -= 1;
            }
            
        }
    }

    public void SetMoveInfo(Vector2 dir)
    {
        transform.rotation = Quaternion.Euler(dir.x, dir.y, 0f);
        moveDirection = dir;
    }

    public void UpDamage(int increase)
    {
        damageIncrease += increase;
    }

    public void UpSpeed(float increase)
    {
        moveSpeed += increase;
    }

    public void UpPenetration(int increase)
    {
        penetrationAmount += increase;
    }

    public int GetDamage() { return baseDamage + damageIncrease; }

    public void ResetValues()
    {
        //Has to be called when the game Ends
        damageIncrease = 0;
        speedIncrease = 0;
        penetrationAmount = 0;
    }
}

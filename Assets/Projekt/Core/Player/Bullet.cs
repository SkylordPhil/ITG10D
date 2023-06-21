using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private int currentDamage;
    [SerializeField] private int currentPenetrationAmount;

    [SerializeField] private float lifeTime = 1f;
    private float currentLifeTime = 0;
    private float lifeTimeIncrease = 0;
    
    private Vector2 moveDirection;

    public PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameManagerController.Instance.getPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        currentDamage = Player.currentDamage;w
        currentPenetrationAmount = Player.currentPenetrationAmount;
        currentMoveSpeed = Player.currentBulletSpeed;

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

    public int GetDamage() { return currentDamage; }
}

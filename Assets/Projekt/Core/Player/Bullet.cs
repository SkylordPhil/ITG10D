using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 2f;


    private float currentLifeTime = 0;
    private Vector2 moveDirection;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        currentLifeTime += Time.deltaTime;
        if(currentLifeTime > lifeTime)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable dmgInterface = collision.gameObject.GetComponent<IDamageable>();

        if (dmgInterface != null)
        {
            dmgInterface.TakeDamage(damage);
        }
    }

    public void SetMoveInfo(Vector2 dir)
    {
        transform.rotation = Quaternion.Euler(dir.x, dir.y, 0f);
        moveDirection = dir;
    }
}

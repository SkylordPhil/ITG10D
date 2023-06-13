using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpecial : MonoBehaviour
{
    public GameObject baseBullet;

    public void baseSpecial()
    {
        Vector3 center = transform.position;
        for (int i = 0; i < 12; i++)
        {
            int a = i * 30;
            Vector3 pos = RandomCircle(center, 1.0f, a);
            GameObject currentBullet = Instantiate(baseBullet, pos, Quaternion.identity);

            Vector3 attackDirection = (Vector3)UnityEngine.Random.insideUnitCircle.normalized;

            currentBullet.transform.position = transform.position;
            currentBullet.GetComponent<Bullet>().SetMoveInfo(attackDirection);
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpecial : MonoBehaviour
{
    private int enemiesToHeal = 50;
    private int enemiesToHealCurrent;
    
    private GameObject player;
    private GameObject[] enemies;

    public void iceSpecial()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemie in enemies)
        {
            if (enemie.GetComponent<BaseEnemy>().frozen == true)
            {
                enemie.GetComponent<BaseEnemy>().Death();
                enemiesToHealCurrent += 1;

                if (enemiesToHealCurrent >= enemiesToHeal)
                {
                    player.GetComponent<PlayerController>().currentHealth += 1;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerSpecial : MonoBehaviour
{
    private GameObject player;
    private GameObject baseBullet;

    private int berserkerNegation;
    private bool wounded;

    public void berserkerSpecial()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        baseBullet = GameObject.FindGameObjectWithTag("Bullet");
        
        //berserkerRage = true;

        float speedIncrease = 2;
        int damageIncrease = baseBullet.GetComponent<Bullet>().GetDamage();

        player.GetComponent<PlayerController>().UpgradeMoveSpeed(speedIncrease);
        baseBullet.GetComponent<Bullet>().UpDamage(damageIncrease);

        //If Time runs out
        if (false)
        {
            player.GetComponent<PlayerController>().UpgradeMoveSpeed(speedIncrease);
            baseBullet.GetComponent<Bullet>().UpDamage(-damageIncrease);

            if (berserkerNegation <= 0)
            {
                wounded = true;
                WoundedStatus();
            }
        }
    }

    private void WoundedStatus()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        baseBullet = GameObject.FindGameObjectWithTag("Bullet");

        float attackSpeedReduction = 1.10f;
        int speedDecrease = -2;

        if (wounded)
        {
            player.GetComponent<PlayerController>().UpgradeAttackSpeed(attackSpeedReduction);
            player.GetComponent<PlayerController>().UpgradeMoveSpeed(speedDecrease);
        }
        else
        {
            player.GetComponent<PlayerController>().UpgradeAttackSpeed(0.90f);
            player.GetComponent<PlayerController>().UpgradeMoveSpeed(-speedDecrease);
        }
    }
}

using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeScriptableObject", menuName ="ScriptableObjects/Upgrade")]
public class UpgradeScriptableObject : ScriptableObject
{
    //Name und Beschreibung
    public string upgradeName;
    public string description;
    public Color color;
    
    //Aktiverungslevel und Upgradebaumzugehörigkeit des Upgrades
    public int level;
    public int tree;
    
    //Values for Bullets
    public int bullets_front;
    public int bullets_back;
    public int bullet_damage;
    public float bullets_speed;
    public int bullet_penetration;

    //Values for the Player
    public float speed;   
    public float fire_rate;
    public int health;
    public bool fire;
    public float fire_dmg;

    //Values for Ravens
    public int raven_amount;
    public float raven_speed;
    public int raven_inventory;

    //Booleans aktivate special
    public bool vulcan;
    public bool blitz;
    public bool cold;
    public bool ice;
    public bool splinter;


}

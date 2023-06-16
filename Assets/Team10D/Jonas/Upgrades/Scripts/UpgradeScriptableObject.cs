using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeScriptableObject", menuName ="ScriptableObjects/Upgrade")]
public class UpgradeScriptableObject : ScriptableObject
{
    //Name und Beschreibung
    [Header("Generelles")] 
    public string upgradeName;
    public string description;
    public Color color;

    //Aktiverungslevel und Upgradebaumzugehörigkeit des Upgrades
    [Header("Struktur")]
    public int level;
    public int tree;

    //Values for Bullets
    [Header("Bullet Stats")]
    public int bullets_front;
    public int bullets_back;
    public int bullet_damage;
    public float bullets_speed;
    public int bullet_penetration;

    //Values for the Player
    [Header("Player Stats")]
    public float speed;   
    public float fire_rate;
    public int health;

    [Header("Fire Stats")]
    public bool fire;
    public float fire_dmg;

    //Values for Ravens
    [Header("Raven Stats")]
    public int raven_amount;
    public float raven_speed;
    public int raven_inventory;
    public bool ravenMagnet;

    //Booleans aktivate special
    [Header("Special")]
    public bool vulcan;

    public bool blitz;
    public int blitzTargets;

    public bool cold;
    public bool ice;
    public bool splinter;


}

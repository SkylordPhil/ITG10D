using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public class PlayerController : MonoBehaviour, IDamageable
{

    [Space(30)]
    [Header("Health")]
    [ContextMenuItem("Damage The Player","DebugTakeDamage")]
    [SerializeField] public int currentHealth;
    [SerializeField] public int currentMaxHealth;
    
    [Space(30)]
    [Header("Raw Player Stats", order = 0)]
    [SerializeField] private float rawMoveSpeed = 5f;
    [SerializeField] private int rawHealth = 3;
    [SerializeField] private float rawAttackSpeed = 1f;
    [SerializeField] private float invulnarableTime = 1f;
    //private bool baseAttack = true;

    [Space(30)]
    [Header("Current Base Stats")]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private int baseHealth;
    [SerializeField] private float baseAttackSpeed;

    [Space(30)]
    [Header("Stat increments")]
    [SerializeField] private float moveSpeedIncrease = 1f;
    [ContextMenuItem("Increase Attack Speed by 20%", "DebugMoreAttackSpeed")]
    [SerializeField] private float attackSpeedIncrease = 1f;

    [Space(30)]
    [Header("Current Stats")]
    [SerializeField] private float currentAttackSpeed;
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private int currentBulletFront;
    [SerializeField] private int currentBulletBack;
    [SerializeField] private int maxRaven;
    [SerializeField] private int currentRavens;
    public int currentLevel;

    [Space(30)]
    [Header("Abilities")]
    public bool fireDmg = false;
    public float fireDmgAmount;
    public float fireTime = 5f;

    public bool firefountain = false;

    public bool lightning = false;
    public int lightningTargets = 2;
    public float lightningDelay = 3;
    
    public bool cold = false;
    public float coldTime = 10;
    public float coldEffect = 0.25f;

    public bool ice = false;
    public float iceMinTime = 5;
    public float iceMaxTime = 10;

    public bool splinters = false;
    public int splintersAnz = 3;

    [Space(30)]
    [Header("LevelProgress")]
    [SerializeField] public int currentXP;
    [SerializeField] public int neededXP;

    [Space(30)]
    [Header("References")]
    [SerializeField] private GameObject baseBullet;
    [SerializeField] private Camera worldCam;
    [SerializeField] private GameObject raven;

    [Space(30)]
    [Header("Controls")]
    [SerializeField] private bool isGamePad;
    public string wah;

    [Space(30)]
    [Header("Player Status")]
    [SerializeField] private bool isInvulnarable;

    [Space(30)]
    [Header("InputControlls")]
    [SerializeField] private InputActionAsset controlls;
    [SerializeField] private InputActionReference moveUp;


    [Space(30)]
    [Header("Debug")]
    [SerializeField] private AudioClip debugClip;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup mixerGroup;

    private bool attackCD;
    private bool attackIsPressed;
    private bool specialIsPressed;

    private Vector2 movement;
    private Vector2 aimVector;
    

    public  InputActionAsset playerControlls;
    private CharacterController2D characterController;

    private InputAction attack;
    private InputAction aimAction;

    //private Dictionary<int, UpgradeScriptableObject> UpgradeDictiorary1 = new Dictionary<int, UpgradeScriptableObject>();

    [Space(30)]
    [Header("Test_Upgrades")]
    [SerializeField] public UpgradeScriptableObject[] allUpgrades;

    private int upgradeTrees_amount = 1;
    private List<UpgradeScriptableObject> selectedUpgrades = new List<UpgradeScriptableObject>();
    private List<UpgradeScriptableObject> choosableUpgrades = new List<UpgradeScriptableObject>();
    public UpgradeScriptableObject[] upgradeSelection = new UpgradeScriptableObject[3];


    /*[Space(30)]
    [Header("Special")]
    [SerializeField] public GameObject[] allSpecials;*/

    private bool baseSpecialBool = false;
    private bool wurfmeisterSpecialBool = false;

    private int enemiesToHeal = 50;
    private int enemiesToHealCurrent;

    private bool berserkerRage = false;
    private int berserkerNegation = 0;
    private bool wounded = false;

    [Space(30)]
    [Header("UI-Elements")]
    public GameObject upgradeUI;
    [SerializeField] private GameObject UI;
    private UIScript UserIntContr;

    /// <summary>
    /// enables all variables and subscribes the controlls to methods
    /// </summary>
    private void Awake()
    {
        SetupStats();

        characterController = GetComponent<CharacterController2D>();
        attack = playerControlls.FindAction("shoot");
        aimAction = playerControlls.FindAction("Aim");
        openMenuAction = playerControlls.FindAction("OpenMenu");
        openMenuAction.performed += OpenMenu;
        attack.performed += AttackAction;
        

        TempActions();

    }

    

    private void Start()
    {
        GameManagerController.Instance.SetPlayer(this);
        worldCam = GameManagerController.Instance.GetCamera();
    }

    /// <summary>
    /// <b>This is not meant to be in the finaly product. </b>
    /// Will be deprecated when the Gamemanager has getPlayer/getCamera functionality
    /// </summary>
    private void TempActions()
    {
        worldCam = FindObjectOfType<Camera>();
    }



    /// <summary>
    /// Assigns base/current stats to their rawValues
    /// </summary>
    private void SetupStats()
    {
        currentMaxHealth = baseHealth = currentHealth = rawHealth;
        currentAttackSpeed = baseAttackSpeed = rawAttackSpeed;
        currentMoveSpeed = baseMoveSpeed = rawMoveSpeed;
    }

    /// <summary>
    /// Enables the playerInput Controls
    /// </summary>
    private void OnEnable()
    {
        playerControlls.Enable();
    }

    /// <summary>
    /// Disables the playerInput Controls
    /// </summary>
    private void OnDisable()
    {
        playerControlls.Disable();
    }
    
    
    
    

    // Update is called once per frame
    void Update()
    {

        HandleInput();
        HandleMovement();
        CheckLvl();

        if (attackIsPressed && !attackCD)
        {
            BaseAttack();
            StartCoroutine(AttackTimer());
        }

        if (lightning)
        {
            lightningDelay -= Time.deltaTime;
            if (lightningDelay <= 0)
            {
                Lightning();
                lightningDelay = 3;
            }
        }
    }

    

    


    
    

    #region PlayerInput


    /// <summary>
    /// Reads movement and aim 2D Vector
    /// </summary>
    void HandleInput()
    {
        movement = playerControlls.FindAction("Movement").ReadValue<Vector2>();
        aimVector = playerControlls.FindAction("Movement").ReadValue<Vector2>();
    }



    /// <summary>
    /// Gets called on ButtonChange of the Attack Button (Pressed and Released)
    /// </summary>
    /// <param name="ctx"></param>
    private void AttackAction(InputAction.CallbackContext ctx)
    {
        attackIsPressed = ctx.control.IsPressed();
    }

    private void OpenMenu(InputAction.CallbackContext obj)
    {
        LevelManager.instance.LoadMenu();
    }


    #endregion

    #region PlayerHealth

    /// <summary>
    /// needs to be implemented when Enemies are integrated
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        if (!isInvulnarable)
        {
            currentHealth -= 1;
            //UserIntContr.UpdateHP(currentHealth, currentMaxHealth);

            if (currentHealth <= 0)
            {
                //Message Gamamaner
                //Kill Player
                return;
            }

            StartCoroutine(DamageTimer());
        }
    }


    /// <summary>
    /// Timer for the invulnarableity frame after getting hit
    /// </summary>
    /// <returns></returns>
    public IEnumerator DamageTimer()
    {
        isInvulnarable = true;
        float timer = 0f;
        while (timer < invulnarableTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        isInvulnarable = false;
    }


    #endregion

    #region Movement

    /// <summary>
    /// Translates the movement Input into PlayerMovement
    /// ATM realy basic and needs refinement
    /// </summary>
    void HandleMovement()
    {
        Vector2 move = new Vector2(movement.x, movement.y);

        characterController.Move(move * Time.deltaTime * currentMoveSpeed);

    }

    #endregion

    #region Attack System

    /// <summary>
    /// At the moment it only takes base raw stats and no Buffs work atm
    /// </summary>
    private void BaseAttack()
    {
        Vector2 attackDirection = ((Vector2)(worldCam.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position)).normalized;

        if (currentBulletFront == 1 || currentBulletFront == 3)
        {
            //Debug.Log("attack1");
            GameObject currentBullet = Instantiate(baseBullet);
            currentBullet.transform.position = transform.position;
            currentBullet.GetComponent<Bullet>().SetMoveInfo(attackDirection);
            DebugSound();
        }

        if (currentBulletFront == 2 || currentBulletFront == 3)
        {
            //Debug.Log("attack2");
            GameObject currentBullet1 = Instantiate(baseBullet);
            currentBullet1.transform.position = transform.position;
            Vector2 attackDirection1 = attackDirection * 1.5f;
            currentBullet1.GetComponent<Bullet>().SetMoveInfo(attackDirection1);

            GameObject currentBullet2 = Instantiate(baseBullet);
            currentBullet2.transform.position = transform.position;
            Vector2 attackDirection2 = attackDirection * 1.75f;
            currentBullet2.GetComponent<Bullet>().SetMoveInfo(attackDirection2);
            DebugSound();
        }


        if (currentBulletBack >= 1)
        {
            //Debug.Log("attack_back");
            GameObject currentBullet = Instantiate(baseBullet);
            currentBullet.transform.position = transform.position;
            currentBullet.GetComponent<Bullet>().SetMoveInfo(-attackDirection);
            DebugSound();
        }
    }

    private void Lightning()
    {
        if (lightning)
        {
            Debug.Log("Blitz");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies != null)
            {
                int rand = UnityEngine.Random.Range(0, enemies.Length / 2);
                int rand2 = UnityEngine.Random.Range(rand, enemies.Length / 2) + rand;

                if (rand2 > enemies.Length)
                {
                    rand2 = enemies.Length;
                }

                int dmg = baseBullet.GetComponent<Bullet>().GetDamage();

                enemies[rand].GetComponent<IDamageable>().TakeDamage(dmg);
                enemies[rand2].GetComponent<IDamageable>().TakeDamage(dmg);
            }
        }
    }

    private void DebugSound()
    {
        AudioManager.GetInstance().PlaySfxAtPosition(this.transform, debugClip, mixerGroup);
    }
    /// <summary>
    /// Enables the bool attack during the Timer
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackTimer()
    {
        attackCD = true;
        float time = 0;

        while (time < 1 / currentAttackSpeed)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        attackCD = false;
    }

    #endregion

    #region SpecialAbilities
    private void baseSpecial()
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

    Vector3 RandomCircle(Vector3 center, float radius, int a)
    {
        Debug.Log(a);
        float ang = a;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    private void IceSpecial()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemie in enemies)
        {
            if (enemie.GetComponent<BaseEnemy>().frozen == true)
            {
                enemie.GetComponent<BaseEnemy>().Death();
                enemiesToHealCurrent += 1;

                if (enemiesToHealCurrent >= enemiesToHeal)
                {
                    currentHealth += 1;
                }
            }
        }
    }

    private void BerserkerSpecial()
    {
        berserkerRage = true;
        
        float speedIncrease = 2;
        int damageIncrease = baseBullet.GetComponent<Bullet>().GetDamage();

        currentMoveSpeed += speedIncrease;
        baseBullet.GetComponent<Bullet>().UpDamage(damageIncrease);

        //If Time runs out
        if (false)
        {
            currentMoveSpeed -= speedIncrease;
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
        float attackSpeedReduction = 1.10f;
        int speedDecrease = -2;

        if (wounded)
        {
            UpgradeAttackSpeed(attackSpeedReduction);
            UpgradeMoveSpeed(speedDecrease);
        }
        else
        {
            UpgradeAttackSpeed(0.90f);
            UpgradeMoveSpeed(-speedDecrease);
        }
    }

    private void RavenSpecial()
    {
        GameObject[] xpOrbs = GameObject.FindGameObjectsWithTag("XP");
        float rangeUp = 999;
        float speedUp = 10;
        foreach (var orb in xpOrbs)
        {
            orb.GetComponent<ExperiencePoint>().ModifyRange(rangeUp);
            orb.GetComponent<ExperiencePoint>().ModifySpeed(speedUp);
            orb.GetComponent<ExperiencePoint>().ravenSpecial = true;
        }
    }

    private void WurfmeisterSpecial()
    {
        //Implement Funktion that allowes to use the base Attack as fast as one can click the attack Button
    }
    #endregion

    #region UpgradeSystem
    public void SelectSelectableUpgrades()
    {
        //Bestimmt Anzahl der gewählten Level 2 Upgrades in einem Upgradebaum (entspricht Arraystelle +1)
        int[] levelTwo = new int[upgradeTrees_amount];
        if (selectedUpgrades.Count > 0)
        {
            foreach(var item in selectedUpgrades)
            {
                if (item.level == 2)
                {
                    levelTwo[item.tree] += 1;
                }
            }
        }

        foreach (var item in allUpgrades)
        {
            if (item.level == 1)
            {
                choosableUpgrades.Add(item);
            }

            if (selectedUpgrades.Count > 0)
            {
                foreach(var obj in selectedUpgrades)
                {
                    if (item.tree == obj.tree && obj.level == 1 && item.level == 2)
                    {
                        choosableUpgrades.Add(item);
                    }
                }
            }

            //Checkt den Upgradebaum des Level 3 Upgrades, sind 2 Level 2 Upgrades vorhanden wird das Upgrade zu den Auswahlmöglichkeiten hinzugefügt
            if (item.level == 3 && levelTwo[item.tree] == 2)
            {
                choosableUpgrades.Add(item);
            }
        }

        foreach (var item in choosableUpgrades.ToList())
        {
            //Implement Loop that removes all Upgrades that are in selectedUpgrades
            foreach (var upgrade in selectedUpgrades)
            {
                if (item.name == upgrade.name)
                {
                    choosableUpgrades.Remove(item);
                }
            }
        }
    }

    public void UpgradePool()
    {
        int amount = choosableUpgrades.Count;
        
        if (amount != 0)
        {
            int rand1 = UnityEngine.Random.Range(0, amount);
            int rand2 = UnityEngine.Random.Range(0, amount);
            int rand3 = UnityEngine.Random.Range(0, amount);

            while (rand1 == rand2 && amount > 1)
            {
                rand2 = UnityEngine.Random.Range(0, amount);
            }
            while ((rand1 == rand3 || rand2 == rand3) && amount > 2)
            {
                rand3 = UnityEngine.Random.Range(0, amount);
            }

            if (amount == 1)
            {
                rand2 = rand1;
                rand3 = rand1;
            }
            else if (amount == 2)
            {
                rand3 = rand1;
            }

            upgradeSelection[0] = choosableUpgrades[rand1];
            upgradeSelection[1] = choosableUpgrades[rand2];
            upgradeSelection[2] = choosableUpgrades[rand3];
        }
    }

    public void AddUpgrade(UpgradeScriptableObject item)
    {
        //Add Item to selectedUpgrades
        selectedUpgrades.Add(item);
        UpgradesAktivieren(item);

        //Remove aktivated Upgrade vom choosableUpgrades-List
        foreach (var upgrade in choosableUpgrades.ToList())
        {
            if (item.name == upgrade.name)
            {
                choosableUpgrades.Remove(upgrade);
            }
        }
    }

    public void UpgradesAktivieren(UpgradeScriptableObject obj)
    {
        UpgradeBulletFront(obj.bullets_front); //BulletFrontUp       
        UpgradeBulletBack(obj.bullets_back); //BulletBackUp
        UpgradeBulletDamage(obj.bullet_damage); //BulletDmgUp
        UpgradeBulletSpeed(obj.bullets_speed); //BulletSpeedUp
        UpgradeBulletPenetration(obj.bullet_penetration); //BulletPenetrationUp
        
        UpgradeMoveSpeed(obj.speed); //MovementSpeedUp
        UpgradeAttackSpeed(obj.fire_rate); //AttackSpeedUp
        UpgradeHealth(obj.health); //HealthUp
        UpgradeFireDamage(obj.fire_dmg); //FireDmgUp
        
        UpgradeRavenAmount(obj.raven_amount); //RavenAmountUp
        UpgradeRavenSpeed(obj.raven_speed); //RavenSpeedUp
        UpgradeRavenInventory(obj.raven_inventory); //RavenInventoryUp

        AktivateVulcano(obj.vulcan);
        AktivatBlitz(obj.blitz);
        AktivateCold(obj.cold);
        AktivateIce(obj.ice);
        AktivateSplinter(obj.splinter);
    }

    public void UpgradeBulletFront(int increase)
    {
        currentBulletFront += increase;
    }

    public void UpgradeBulletBack(int increase)
    {
        currentBulletBack += increase;
    }

    public void UpgradeBulletDamage(int increase)
    {
        baseBullet.GetComponent<Bullet>().UpDamage(increase);
    }

    public void UpgradeBulletSpeed(float increase) 
    {
        baseBullet.GetComponent<Bullet>().UpSpeed(increase);
    }

    public void UpgradeBulletPenetration(int increase)
    {
        baseBullet.GetComponent<Bullet>().UpPenetration(increase);
    }

    public void UpgradeMoveSpeed(float increase)
    {
        moveSpeedIncrease += increase;
        currentMoveSpeed = moveSpeedIncrease * baseMoveSpeed;
    }

    public void UpgradeAttackSpeed(float increase)
    {
        attackSpeedIncrease += increase;
        currentAttackSpeed = attackSpeedIncrease * baseAttackSpeed;

    }

    public void UpgradeHealth(int increase)
    {
        currentHealth += increase;
        currentMaxHealth += increase;
    }

    public void UpgradeFireDamage(float increase)
    {
        if (increase > 0)
        {
            fireDmg = true;
            fireDmgAmount += increase;
        }
    }

    public void UpgradeRavenAmount(int increase)
    {
        maxRaven += increase;

        Vector2 currentPos = transform.position;
        for (int i = 0; i < maxRaven - currentRavens; i++)
        {
            Debug.Log("Raven");
            Instantiate(raven, currentPos, Quaternion.identity);
            currentRavens++;
        }
    }

    public void UpgradeRavenSpeed(float increase)
    {
        raven.GetComponent<RavenBase>().SpeedUp(increase);
    }

    public void UpgradeRavenInventory(int increase)
    {
        raven.GetComponent<RavenBase>().InventoryUp(increase);
    }

    public void AktivateVulcano(bool aktivate)
    {
        if (aktivate)
        {
            firefountain = true;
        }
    }

    public void AktivatBlitz(bool aktivate)
    {
        if (aktivate)
        {
            lightning = true;
        }
    }

    public void AktivateCold(bool aktivate)
    {
        if (aktivate)
        {
            cold = true;
        }
    }

    public void AktivateIce(bool aktivate)
    {
        if (aktivate)
        {
            ice = true;
        }
    }

    public void AktivateSplinter(bool aktivate)
    {
        if (aktivate)
        {
            splinters = true;
        }
    }

    #endregion

    #region LevelSystem
    private void CheckLvl()
    {
        if (currentXP >= neededXP)
        {
            LevelUp(currentLevel);
        }
    }
    
    private void LevelUp(int lvl)
    {
        int baseValue = 50;
        int mathLvl = lvl + 1;
        float power = 1.1f;

        neededXP = baseValue * (int)Mathf.Pow(mathLvl, power);
        currentXP = 0;
        currentLevel += 1;

        SelectSelectableUpgrades();
        UpgradePool();

        Instantiate(upgradeUI);
        Time.timeScale = 0;
    }
    
    internal void GetXP(int v)
    {
        currentXP += v;
    }
    #endregion

    #region DEBUG 
    public void DebugTakeDamage()
    {
        if (!isInvulnarable)
        {
            currentHealth -= 1;

            if (currentHealth == 0)
            {
                //Message Gamamaner
                //Kill Player
                return;
            }

            StartCoroutine(DamageTimer());
        }
    }

    public void DebugMoreAttackSpeed()
    {
        UpgradeAttackSpeed(0.20f);
    }


    #endregion
    
}

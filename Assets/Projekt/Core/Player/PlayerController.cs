using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Progress;


public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Health")]
    //[ContextMenuItem("Damage The Player","DebugTakeDamage")]
    [SerializeField] public int currentHealth;
    [SerializeField] public int currentMaxHealth;
    [SerializeField] private int rawHealth = 3;

    [Header("Movement Speed")]
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float moveSpeedIncrease = 1f;
    [SerializeField] private float rawMoveSpeed = 5f;

    [Header("Attack Speed")]
    [SerializeField] private float rawAttackSpeed = 1f;
    [SerializeField] private float attackSpeedIncrease = 1f;
    [SerializeField] private float currentAttackSpeed;

    [Header("Level")]
    public int currentLevel;
    public int currentXP;
    public int neededXP;

    [Header("Invulnarability")]
    [SerializeField] private float invulnarableTime = 1f;
    [SerializeField] private bool isInvulnarable;

    [Header("Bullets")]
    [SerializeField] private int currentBulletFront;
    [SerializeField] private int currentBulletBack;
    [Space(5)]
    [SerializeField] public int currentDamage;
    private int baseDamage = 3;
    private int damageIncrease;
    [Space(5)]
    [SerializeField] public int currentPenetrationAmount;
    private int penetrationAmount = 0;
    private int penetrationAmountIncrease = 0;
    [Space(5)]
    [SerializeField] public float currentBulletSpeed;
    private float bulletSpeed = 10f;
    private int bulletSpeedIncrease = 0;

    [Header("Attack Cooldown")]
    private bool attackCD;
    private bool attackIsPressed;
    private bool specialAttackCD;
    private bool specialIsPressed;

    [Header("Raven")]
    [SerializeField] private int maxRaven;
    [SerializeField] private int currentRavens;
    [Space(5)]
    private float ravenSpeed = 8f;
    private float ravenSpeedIncrease;
    public float ravenSpeedCurrent;
    [Space(5)]
    private int ravenInventoryLimitBase = 5;
    public int ravenInventoryLimitCurrent;
    private int ravenInventoryLimitIncrease;

    [Header("Abilities")]
    public bool fireDmg = false;
    public float fireDmgAmount;
    public float fireTime = 5f;
    [Space(5)]
    public bool firefountain = false;
    [Space(5)]
    public bool lightning = false;
    private int lightningTargets = 1;
    private float lightningDelay = 10;
    [Space(5)]
    public bool cold = false;
    public float coldTime = 10;
    public float coldEffect = 1f;
    [Space(5)]
    public bool ice = false;
    public float iceMinTime = 5;
    public float iceMaxTime = 10;
    [Space(5)]
    public bool splinters = false;
    public int splintersAnz = 3;
    [Space(5)]
    public bool iceSpecial = false;
    private int enemiesToHeal = 50;
    public int enemiesToHealCurrent;

    [Header("Upgrades")]
    [SerializeField] public UpgradeScriptableObject[] allUpgrades;
    [Space(5)]
    [SerializeField] private int upgradeTrees_amount = 7;
    private List<UpgradeScriptableObject> selectedUpgrades = new List<UpgradeScriptableObject>();
    private List<UpgradeScriptableObject> choosableUpgrades = new List<UpgradeScriptableObject>();
    public UpgradeScriptableObject[] upgradeSelection = new UpgradeScriptableObject[3];

    [Header("UI-Elements")]
    public GameObject upgradeUI;
    [SerializeField] private GameObject UI;
    [SerializeField] private UIScript uiscriptelement;
    private UIScript UserIntContr;

    [Header("References")]
    [SerializeField] private GameObject baseBullet;
    [SerializeField] private Camera worldCam;
    [SerializeField] private GameObject raven;
    [SerializeField] private SpriteRenderer playerSprite;
    
    [Header("Controls")]
    [SerializeField] private bool isGamePad;
    public string wah;
    
    [Header("InputControlls")]
    [SerializeField] private InputActionAsset controlls;
    [SerializeField] private InputActionReference moveUp;
    public InputActionAsset playerControlls;
    private CharacterController2D characterController;

    [Header("InputActions")]
    private InputAction attack;
    private InputAction specialAttack;
    private InputAction aimAction;
    private InputAction openMenuAction;

    [Header("Debug")]
    [SerializeField] private AudioClip debugClip;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup mixerGroup;
    
    [Header("Vectors")]
    private Vector2 movement;
    private Vector2 aimVector;

    /* Auskommentiert da nicht implementiert
    [Space(30)]
    [Header("Special")]
    //Not Implemented
    private bool baseSpecialBool = false;
    private bool wurfmeisterSpecialBool = false;

    private bool berserkerRage = false;
    private int berserkerNegation = 0;
    private bool wounded = false;
    */

    /// <summary>
    /// enables all variables and subscribes the controlls to methods
    /// </summary>
    private void Awake()
    {
        SetupStats();

        characterController = GetComponent<CharacterController2D>();
        attack = playerControlls.FindAction("shoot");
        specialAttack = playerControlls.FindAction("Special");
        aimAction = playerControlls.FindAction("Aim");
        openMenuAction = playerControlls.FindAction("OpenMenu");
        openMenuAction.performed += OpenMenu;
        attack.performed += AttackAction;
        specialAttack.performed += SpecialAttackAction;
        specialAttack.canceled += SpecialAttackAction;

        
        TempActions();
    }

    private void Start()
    {
        GameManagerController.Instance.SetPlayer(this);
        worldCam = CameraController.Instance.gameObject.GetComponent<Camera>();
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
        currentMaxHealth = currentHealth = rawHealth;
        currentAttackSpeed = rawAttackSpeed;
        currentMoveSpeed = rawMoveSpeed;

        currentDamage = baseDamage;
        currentPenetrationAmount = penetrationAmount;
        currentBulletSpeed = bulletSpeed;

        ravenInventoryLimitCurrent = ravenInventoryLimitBase;
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

        if (specialIsPressed && !specialAttackCD)
        {
            baseSpecial();
            StartCoroutine(SpecialAttackTimer());
        }

        if (lightning)
        {
            lightningDelay -= Time.deltaTime;
            if (lightningDelay <= 0)
            {
                for (int i = lightningTargets; i >= 0; i--)
                {
                    Lightning();
                }
                lightningDelay = 3;
            }
        }

        if (iceSpecial)
        {
            IceSpecial();
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

    private void SpecialAttackAction(InputAction.CallbackContext ctx)
    {
        specialIsPressed = ctx.control.IsPressed();
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
                //Message Gamemanager
                GameManagerController.Instance.GameOver();
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

        if (movement.x > 0)
        {
            playerSprite.flipX = true;
        }
        else if (movement.x < 0)
        {
            playerSprite.flipX = false;
        }
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

            if (enemies != null && enemies.Length >= lightningTargets)
            {
                int rand = UnityEngine.Random.Range(0, enemies.Length / 2);
                int dmg = currentDamage;

                enemies[rand].GetComponent<BaseEnemy>().lightningEffect.Play();
                enemies[rand].GetComponent<IDamageable>().TakeDamage(dmg);
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

    IEnumerator SpecialAttackTimer()
    {
        specialAttackCD = true;
        uiscriptelement.ChangeSpecialCooldownVisibility();
        float time = 30;

        while(time > 0)
        {
            time -= Time.deltaTime;
            uiscriptelement.ChangeCooldownTime(MathF.Round(time));
            yield return new WaitForEndOfFrame();
        }
        uiscriptelement.ChangeSpecialCooldownVisibility();
        specialAttackCD = false;
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
        if (enemiesToHealCurrent >= enemiesToHeal)
        {
            currentHealth += 1;
            enemiesToHealCurrent -= enemiesToHeal;
        }
    }

    /* Auskommentiert, da noch nicht implementiert
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
    */
    #endregion

    #region UpgradeSystem
    public void SelectSelectableUpgrades()
    {
        //Bestimmt Anzahl der gewählten Level 2 Upgrades in einem Upgradebaum (entspricht Arraystelle +1)
        int[] levelTwo = new int[upgradeTrees_amount+1];
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

                    Debug.Log("Removed Upgrade: " + item.name);
                    /*foreach (var i in choosableUpgrades)
                    {
                        Debug.Log(i);
                    }*/
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

    private void UpgradesAktivieren(UpgradeScriptableObject obj)
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
        AktivateRavenMagnet(obj.ravenMagnet);

        AktivateVulcano(obj.vulcan);
        AktivatBlitz(obj.blitz);
        UpgradeBlitzAmount(obj.blitzTargets);
        AktivateCold(obj.cold);
        AktivateIce(obj.ice);
        AktivateSplinter(obj.splinter);
        AktivateIceSpecial(obj.iceHealing);
    }

    private void UpgradeBulletFront(int increase)
    {
        currentBulletFront += increase;
    }

    private void UpgradeBulletBack(int increase)
    {
        currentBulletBack += increase;
    }

    private void UpgradeBulletDamage(int increase)
    {
        damageIncrease += increase;
        currentDamage = baseDamage + damageIncrease;

    }
    private void UpgradeBulletSpeed(float increase) 
    {
        bulletSpeedIncrease += (int)increase;
        currentBulletSpeed = bulletSpeed + bulletSpeedIncrease;
    }

    private void UpgradeBulletPenetration(int increase)
    {
        penetrationAmountIncrease += increase;
        currentPenetrationAmount = penetrationAmount + penetrationAmountIncrease;
    }

    private void UpgradeMoveSpeed(float increase)
    {
        moveSpeedIncrease += increase;
        currentMoveSpeed = rawMoveSpeed * moveSpeedIncrease;
    }

    private void UpgradeAttackSpeed(float increase)
    {
        attackSpeedIncrease += increase;
        currentAttackSpeed = attackSpeedIncrease * rawAttackSpeed;

    }

    private void UpgradeHealth(int increase)
    {
        currentHealth += increase;
        currentMaxHealth += increase;
    }

    private void UpgradeFireDamage(float increase)
    {
        if (increase > 0)
        {
            fireDmg = true;
            fireDmgAmount += increase;
        }
    }

    private void UpgradeRavenAmount(int increase)
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

    private void UpgradeRavenSpeed(float increase)
    {
        ravenSpeedIncrease += increase;
        ravenSpeedCurrent = ravenSpeed + ravenSpeedIncrease;
    }

    private void UpgradeRavenInventory(int increase)
    {
        ravenInventoryLimitIncrease += increase;
        ravenInventoryLimitCurrent = ravenInventoryLimitBase + ravenInventoryLimitIncrease;
    }

    private void AktivateRavenMagnet(bool aktivate)
    {
        if (aktivate)
        {
            raven.GetComponent<RavenBase>().EnablePickup();
        }
    }

    private void AktivateVulcano(bool aktivate)
    {
        if (aktivate)
        {
            firefountain = true;
        }
    }

    private void AktivatBlitz(bool aktivate)
    {
        if (aktivate)
        {
            lightning = true;
        }
    }

    private void UpgradeBlitzAmount(int increase)
    {
        lightningTargets += increase;
    }

    private void AktivateCold(bool aktivate)
    {
        if (aktivate)
        {
            cold = true;
        }
    }

    private void AktivateIce(bool aktivate)
    {
        if (aktivate)
        {
            ice = true;
        }
    }

    private void AktivateSplinter(bool aktivate)
    {
        if (aktivate)
        {
            splinters = true;
        }
    }

    private void AktivateIceSpecial(bool aktivate)
    {
        if (aktivate)
        {
            iceSpecial = true;
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

        neededXP = baseValue * mathLvl;
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

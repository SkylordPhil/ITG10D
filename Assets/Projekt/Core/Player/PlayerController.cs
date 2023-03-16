using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{

    [Space(30)]
    [Header("Health")]
    [ContextMenuItem("Damage The Player","DebugTakeDamage")]
    [SerializeField] private int currentHealth;
    [SerializeField] private int currentMaxHealth;
    [Space(30)]
    [Header("Raw Player Stats", order = 0)]
    [SerializeField] private float rawMoveSpeed = 5f;

    

    [SerializeField] private int rawHealth = 3;
    [SerializeField] private float rawAttackSpeed = 1f;
    [SerializeField] private float invulnarableTime = 1f;

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

    [Space(30)]
    [Header("References")]
    [SerializeField] private GameObject baseBullet;
    [SerializeField] private Camera worldCam;

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

    private Vector2 movement;
    private Vector2 aimVector;
    

    public  InputActionAsset playerControlls;
    private CharacterController2D characterController;



    private InputAction attack;
    private InputAction aimAction;

    /// <summary>
    /// enables all variables and subscribes the controlls to methods
    /// </summary>
    private void Awake()
    {
        SetupStats();

        characterController = GetComponent<CharacterController2D>();
        attack = playerControlls.FindAction("shoot");
        aimAction = playerControlls.FindAction("Aim");
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

        if(attackIsPressed && !attackCD)
        {
            BaseAttack();
            StartCoroutine(AttackTimer());

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


        Debug.Log("attack");
        Vector2 attackDirection = ((Vector2)(worldCam.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position)).normalized;
        GameObject currentBullet = Instantiate(baseBullet);
        currentBullet.transform.position = transform.position;
        currentBullet.GetComponent<Bullet>().SetMoveInfo(attackDirection);
        DebugSound();


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

    #region UpgradeSystem

    public void UpgradeAttackSpeed(float increase)
    {
        attackSpeedIncrease += increase;
        currentAttackSpeed = attackSpeedIncrease * baseAttackSpeed;

    }

    public void UpgradeMoveSpeed(float increase)
    {
        moveSpeedIncrease += increase;
        currentMoveSpeed = moveSpeedIncrease * baseMoveSpeed;
    }




    #endregion

    #region LevelSystem
    internal void GetXP(int v)
    {
        throw new NotImplementedException();
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

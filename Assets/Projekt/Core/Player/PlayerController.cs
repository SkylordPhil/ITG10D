using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float rawPlayerSpeed = 5f;
    [SerializeField] private int baseHealth = 3;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private bool isGamePad;
    [SerializeField] private GameObject baseBullet;

    [SerializeField] private Camera worldCam;


    private bool attackCD;
    private bool attackIsPressed;

    private Vector2 movement;
    private Vector2 aimVector;
    

    private PlayerControlls playerControlls;
    private CharacterController2D characterController;
    private PlayerInput playerInput;


    private InputAction attack;
    private InputAction aimAction;

    /// <summary>
    /// enables all variables and subscribes the controlls to methods
    /// </summary>
    private void Awake()
    {
        playerControlls = new PlayerControlls();
        characterController = GetComponent<CharacterController2D>();
        playerInput = GetComponent<PlayerInput>();
        attack = playerControlls.Controlls.Shoot;
        aimAction = playerControlls.Controlls.Aim;
        attack.performed += AttackAction;

        TempActions();

    }

    /// <summary>
    /// This is not meant to be in the finaly product.
    /// Will be deprecated when the Gamemanage has a getPlayer/getCamera functionality
    /// </summary>
    private void TempActions()
    {
        worldCam = FindObjectOfType<Camera>();
    }
    
    /// <summary>
    /// Gets called on ButtonChange of the Attack Button (Pressed and Released)
    /// </summary>
    /// <param name="ctx"></param>
    private void AttackAction(InputAction.CallbackContext ctx)
    {
        attackIsPressed = ctx.control.IsPressed();
    }

    /// <summary>
    /// At the moment it only takes base raw stats and no Buffs work atm
    /// </summary>
    private void BaseAttack()
    {
        Debug.Log("attack");
        Vector2 attackDirection = ((Vector2)(worldCam.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position)).normalized;
        GameObject currentBullet = Instantiate(baseBullet);
        currentBullet.transform.position = transform.position;
        currentBullet.GetComponent<Bullet>().getMoveInfo(attackDirection);

        
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
    
    /// <summary>
    /// Reads movement and aim 2D Vector
    /// </summary>
    void HandleInput()
    {
        movement = playerControlls.Controlls.Movement.ReadValue<Vector2>();
        aimVector = playerControlls.Controlls.Aim.ReadValue<Vector2>();
    }

    /// <summary>
    /// Translates the movement Input into PlayerMovement
    /// ATM realy basic and needs refinement
    /// </summary>
    void HandleMovement()
    {
        Vector2 move = new Vector2(movement.x, movement.y);

        characterController.Move(move * Time.deltaTime * rawPlayerSpeed);

    }

    // Update is called once per frame
    void Update()
    {

        HandleInput();
        HandleMovement();

        if(attackIsPressed && !attackCD)
        {
            BaseAttack();
            StartCoroutine(attackTimer());

        }
    }

    /// <summary>
    /// Enables the bool attack during the Timer
    /// </summary>
    /// <returns></returns>
    IEnumerator attackTimer()
    {
        attackCD = true;
        float time = 0;

        while(time < 1/attackSpeed)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();

        }
        attackCD = false;
    }

    



    /// <summary>
    /// needs to be implemented when Enemies are integrated
    /// </summary>
    /// <param name="dmg"></param>
    public void takeDamage(int dmg)
    {
    }
}

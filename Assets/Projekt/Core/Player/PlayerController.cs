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


    private void Awake()
    {
        playerControlls = new PlayerControlls();
        characterController = GetComponent<CharacterController2D>();
        playerInput = GetComponent<PlayerInput>();
        attack = playerControlls.Controlls.Shoot;
        aimAction = playerControlls.Controlls.Aim;
        attack.performed += AttackAction;

        tempActions();

    }

    private void tempActions()
    {
        worldCam = FindObjectOfType<Camera>();
    }
    

    private void AttackAction(InputAction.CallbackContext ctx)
    {
        attackIsPressed = ctx.control.IsPressed();
    }

    private void BaseAttack()
    {
        Debug.Log("attack");
        Vector2 attackDirection = ((Vector2)(worldCam.ScreenToWorldPoint(aimAction.ReadValue<Vector2>()) - transform.position)).normalized;
        GameObject currentBullet = Instantiate(baseBullet);
        currentBullet.transform.position = transform.position;
        currentBullet.GetComponent<Bullet>().getMoveInfo(attackDirection);

        
    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }
    
    void HandleInput()
    {
        movement = playerControlls.Controlls.Movement.ReadValue<Vector2>();
        aimVector = playerControlls.Controlls.Aim.ReadValue<Vector2>();
    }
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

    




    public void takeDamage(int dmg)
    {
    }
}

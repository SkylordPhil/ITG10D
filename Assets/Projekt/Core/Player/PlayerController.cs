using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rawPlayerSpeed = 5f;
    [SerializeField] private bool isGamePad;


    private Vector2 movement;
    private Vector2 aim;

    private PlayerControlls playerControlls;
    private CharacterController2D characterController;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerControlls = new PlayerControlls();
        characterController = GetComponent<CharacterController2D>();
        playerInput = GetComponent<PlayerInput>();
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
        aim = playerControlls.Controlls.Aim.ReadValue<Vector2>();
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
    }


    
}

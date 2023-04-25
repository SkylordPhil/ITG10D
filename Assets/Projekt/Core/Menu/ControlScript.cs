using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public enum Controls { moveUp, moveDown, moveLeft, moveRight, mainAttack, specialAbility };

public class ControlScript : BaseSaveScript
{
    [SerializeField] private Controls controls;
    [SerializeField] private TextMeshProUGUI textField;

    private bool isPressed = false;

    public InputActionAsset inputActionAsset;



    public void Start()
    {
        string[] textArray = { };

        switch (controls)
        {
            case Controls.moveUp:
                textArray = settingsData.moveUp.Split("/");
                break;

            case Controls.moveDown:
                textArray = settingsData.moveDown.Split("/");
                break;

            case Controls.moveLeft:
                textArray = settingsData.moveLeft.Split("/");
                break;

            case Controls.moveRight:
                textArray = settingsData.moveRight.Split("/");
                break;

            case Controls.mainAttack:
                textArray = settingsData.mainAttack.Split("/");
                break;

            case Controls.specialAbility:
                textArray = settingsData.specialAbility.Split("/");
                break;
        }
        
        string vartext = textArray[textArray.Length - 1];
        vartext = vartext[0].ToString().ToUpper() + vartext.Substring(1);
        textField.text = vartext;
    }


    public void InitializeKeyChange()
    {
            textField.text = "Press Button";            

            switch (controls)
            {
                case Controls.moveUp:
                case Controls.moveDown:
                case Controls.moveLeft:
                case Controls.moveRight:
                    inputActionAsset.FindAction("Movement").PerformInteractiveRebinding(1).WithExpectedControlType("Button").OnMatchWaitForAnother(0.1f).OnComplete(operation =>
                    {
                        saveAndDisplayNewKeybind(operation.selectedControl);
                        operation.Dispose();
                    }).Start();
                break;

                case Controls.mainAttack:
                    inputActionAsset.FindAction("Shoot").PerformInteractiveRebinding().OnMatchWaitForAnother(0.1f).OnComplete(operation =>
                    {
                        saveAndDisplayNewKeybind(operation.selectedControl);
                        operation.Dispose();
                    }).Start();
                    break;

                case Controls.specialAbility:
                    inputActionAsset.FindAction("special").PerformInteractiveRebinding().OnMatchWaitForAnother(0.1f).OnComplete(operation =>
                    {
                        saveAndDisplayNewKeybind(operation.selectedControl);
                        operation.Dispose();
                    }).Start();
                    break;
            }

    }

    private void saveAndDisplayNewKeybind(InputControl key)
    {
        switch (controls) 
        {
            case Controls.moveUp:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", key.path)
                    .With("Down", settingsData.moveDown)
                    .With("Left", settingsData.moveLeft)
                    .With("Right", settingsData.moveRight);
                settingsData.p_moveUp = key.path;
                break;

            case Controls.moveDown:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", key.path)
                    .With("Left", settingsData.moveLeft)
                    .With("Right", settingsData.moveRight);
                settingsData.p_moveDown = key.path;
                break;

            case Controls.moveLeft:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", settingsData.moveDown)
                    .With("Left", key.path)
                    .With("Right", settingsData.moveRight);
                settingsData.p_moveLeft = key.path;
                break;

            case Controls.moveRight:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", settingsData.moveDown)
                    .With("Left", settingsData.moveLeft)
                    .With("Right", key.path);
                settingsData.p_moveRight = key.path;
                break;

            case Controls.mainAttack:
                inputActionAsset.FindAction("shoot").ApplyBindingOverride(key.path);
                settingsData.p_mainAttack = key.path;
                break;

            case Controls.specialAbility:
                inputActionAsset.FindAction("special").ApplyBindingOverride(key.path);
                settingsData.p_specialAbility = key.path;
                break;
        }

        textField.text = key.displayName;
    }
    
}

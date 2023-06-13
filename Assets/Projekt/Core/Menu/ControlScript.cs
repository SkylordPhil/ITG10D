using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public enum Controls { moveUp, moveDown, moveLeft, moveRight, mainAttack, specialAbility };

public class ControlScript : BaseSaveScript
{
    [SerializeField] private Controls controls;
    [SerializeField] private TextMeshProUGUI textField;

    public InputActionAsset inputActionAsset;



    public void Start()
    {

        switch (controls)
        {
            case Controls.moveUp:
                textField.text = settingsData.moveUp;
                break;

            case Controls.moveDown:
                textField.text = settingsData.moveDown;
                break;

            case Controls.moveLeft:
                textField.text = settingsData.moveLeft;
                break;

            case Controls.moveRight:
                textField.text = settingsData.moveRight;
                break;

            case Controls.mainAttack:
                textField.text = settingsData.mainAttack;
                break;

            case Controls.specialAbility:
                textField.text = settingsData.specialAbility;
                break;
        }
    }


    public void InitializeKeyChange()
    {
            isPressed = true;
            textField.text = "Press Button";            

            Debug.Log("test");
            switch (controls)
            {
                case Controls.moveUp:
                    textField.text = settingsData.moveUp;
                    break;

                case Controls.moveDown:
                    textField.text = settingsData.moveDown;
                    break;

                case Controls.moveLeft:
                    textField.text = settingsData.moveLeft;
                    break;

                case Controls.moveRight:
                    textField.text = settingsData.moveRight;
                    break;

                case Controls.mainAttack:
                    inputActionAsset.FindAction("Shoot").PerformInteractiveRebinding().OnMatchWaitForAnother(0.1f).OnComplete(operation =>
                    {
                        string newKeybind = inputActionAsset.FindAction("Shoot").GetBindingDisplayString();
                        Debug.Log(newKeybind);
                        settingsData.mainAttack = newKeybind;
                        textField.text = newKeybind;
                    }).Start();
                    textField.text = settingsData.mainAttack;
                    break;

                case Controls.specialAbility:
                    textField.text = settingsData.specialAbility;
                    break;
            }

            isPressed = false;
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
                settingsData.moveUp = key.displayName;
                break;

            case Controls.moveDown:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", key.path)
                    .With("Left", settingsData.moveLeft)
                    .With("Right", settingsData.moveRight);
                settingsData.moveDown = key.displayName;
                break;

            case Controls.moveLeft:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", settingsData.moveDown)
                    .With("Left", key.path)
                    .With("Right", settingsData.moveRight);
                settingsData.moveLeft = key.displayName;
                break;

            case Controls.moveRight:
                inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
                inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
                    .With("Up", settingsData.moveUp)
                    .With("Down", settingsData.moveDown)
                    .With("Left", settingsData.moveLeft)
                    .With("Right", key.path);
                settingsData.moveRight = key.displayName;
                break;

            case Controls.mainAttack:
                inputActionAsset.FindAction("shoot").ApplyBindingOverride(key.path);
                settingsData.mainAttack = key.displayName;
                break;

            case Controls.specialAbility:
                inputActionAsset.FindAction("special").ApplyBindingOverride(key.path);
                settingsData.specialAbility = key.displayName;
                break;
        }

        textField.text = key.displayName;
    }
    
}

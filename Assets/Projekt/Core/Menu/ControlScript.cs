using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public enum Controls { MoveUp, MoveDown, MoveLeft, MoveRight, MainAttack, SpecialAbility };

public class ControlScript : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private Controls controls;
    [SerializeField] private TextMeshProUGUI textField;

    private bool isPressed = false;

    private InputAction inputListener = new InputAction(binding: "/*/<button>");
    public InputActionAsset action; 

    public void Start()
    {
        inputListener.performed -= InputListener_performed;
        inputListener.performed += InputListener_performed;

        switch (controls)
        {
            case Controls.MoveUp:
                textField.text = settingsData.moveUp;
                break;

            case Controls.MoveDown:
                textField.text = settingsData.moveDown;
                break;

            case Controls.MoveLeft:
                textField.text = settingsData.moveLeft;
                break;

            case Controls.MoveRight:
                textField.text = settingsData.moveRight;
                break;

            case Controls.MainAttack:
                textField.text = settingsData.mainAttack;
                break;

            case Controls.SpecialAbility:
                textField.text = settingsData.specialAbility;
                break;
        }
    }

    //executed if any key has been pressed while the input listener is enabled
    private void InputListener_performed(InputAction.CallbackContext obj)
    {
        if (obj.control.displayName == "Any Key" || obj.control.displayName == "Press")
        {
            return;
        }
        string pressedKey = obj.control.displayName;
        Debug.Log(obj.control.displayName);
        saveAndDisplayNewKeybind(obj.control);
        
        inputListener.Disable();
       
    }

    public void InitializeKeyChange()
    {
        if (!isPressed)
        {
            isPressed = true;
            textField.text = "Press Button";
            inputListener.Enable();
            
        }
        else
        {
            isPressed = false;
            inputListener.Disable();
        }
    }

    private void saveAndDisplayNewKeybind(InputControl key)
    {
        switch (controls) 
        {
            case Controls.MoveUp:
                break;

            case Controls.MoveDown:
                break;

            case Controls.MoveLeft:
                break;

            case Controls.MoveRight:
                
                break;

            case Controls.MainAttack:
                action.FindAction("shoot").ApplyBindingOverride(key.path);
                break;

            case Controls.SpecialAbility:
                action.FindAction("special").ApplyBindingOverride(key.path);
                break;
        }

        textField.text = key.displayName;
    }
    
}

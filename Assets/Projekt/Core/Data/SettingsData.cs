using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
[CreateAssetMenu(fileName = "settings", menuName = "Settings/SettingsObject", order = 1)]
public class SettingsData: ScriptableObject
{
    public string p_moveUp { 
        get { return moveUp; }
        set 
        { 
            moveUp = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }

    public string p_moveDown
    {
        get { return moveDown; }
        set
        {
            moveDown = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }

    public string p_moveLeft
    {
        get { return moveLeft; }
        set
        {
            moveLeft = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }

    public string p_moveRight
    {
        get { return moveRight; }
        set
        {
            moveRight = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }

    public string p_mainAttack
    {
        get { return mainAttack; }
        set
        {
            mainAttack = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }

    public string p_specialAbility
    {
        get { return specialAbility; }
        set
        {
            specialAbility = value;
            LoadAndSaveJson.Instance.saveSettings();
        }
    }



    //Settings
    public string moveUp;
    public string moveDown;
    public string moveLeft;
    public string moveRight;
    public string mainAttack;
    public string specialAbility;
    public int masterVolumeValue;
    public int musicVolumeValue;
    public int sfxVolumeValue;

    public void SetDefaultData()
    {
        moveUp = "/Keyboard/w";
        moveDown = "/Keyboard/s";
        moveLeft = "/Keyboard/a";
        moveRight = "/Keyboard/d";
        mainAttack = "/Mouse/leftButton";
        specialAbility = "/Mouse/rightButton";
        masterVolumeValue = 50;
        musicVolumeValue = 50;
        sfxVolumeValue = 50;
    }
    
}

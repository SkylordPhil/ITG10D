using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[Serializable]
[CreateAssetMenu(fileName = "settings", menuName = "Settings/SettingsObject", order = 1)]
public class SettingsData: ScriptableObject
{
    private string path;

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
        moveUp = "W";
        moveDown = "S";
        moveLeft = "A";
        moveRight = "D";
        mainAttack = "Left Button";
        specialAbility = "Right Button";
        masterVolumeValue = 50;
        musicVolumeValue = 50;
        sfxVolumeValue = 50;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsData
{
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

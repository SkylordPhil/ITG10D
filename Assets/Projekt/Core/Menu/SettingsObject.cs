using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SettingsObject", menuName = "Settings", order = 1)]
public class SettingsObject : ScriptableObject
{
    public int masterVolumeValue;
    public int musicVolumeValue;
    public int sfxVolumeValue;
}

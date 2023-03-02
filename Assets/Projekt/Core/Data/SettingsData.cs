using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData : MonoBehaviour
{
    private int masterVolumeValue;
    private int musicVolumeValue;
    private int sfxVolumeValue;

    public void SetSettingsData(int masterVolumeValue, int musicVolumeValue, int sfxVolumeValue)
    {
        this.masterVolumeValue = masterVolumeValue;
        this.musicVolumeValue = musicVolumeValue;
        this.sfxVolumeValue = sfxVolumeValue;
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;
using TMPro;

public class SliderScript : BaseSaveScript
{
    [SerializeField] private MenuType menuType;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private string audioMenuType;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Start()
    { 

        switch (audioMenuType)
        {
            case "masterVolume":
                slider.value = settingsData.masterVolumeValue;
                break;

            case "musicVolume":
                slider.value = settingsData.musicVolumeValue;
                break;

            case "sfxVolume":
                slider.value = settingsData.sfxVolumeValue;
                break;
        }
    }

    public void OnChange()
    {
        float convertedSliderValue = slider.value / 100;
        menuManager.DetermineMenuType(menuType, convertedSliderValue, audioMenuType);
        textMesh.text = slider.value.ToString();
        var currentlySavedValue = settingsData.GetType().GetProperty(audioMenuType).GetValue(settingsData);
        Debug.Log("currentlySavedValue");
        if (slider.value != (int)settingsData.GetType().GetProperty(audioMenuType).GetValue(settingsData))
        {
            Debug.Log("WOrked");
        }
    }
}

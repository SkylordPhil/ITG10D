using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    [SerializeField] private MenuType menuType;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private string stringParam;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Start()
    {
        slider = this.gameObject.GetComponent<Slider>(); 

        switch (stringParam)
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
        menuManager.DetermineMenuType(menuType, convertedSliderValue, stringParam);
        textMesh.text = slider.value.ToString();
    }
}

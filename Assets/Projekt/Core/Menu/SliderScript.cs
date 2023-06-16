using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private MenuType menuType;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private string audioMenuType;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Start()
    {
        Debug.Log(AudioManager.GetInstance());
        int currentVolumeValue = AudioManager.GetInstance().GetExposedParamValue(audioMenuType);
        Debug.Log("currentVolumeValue from SliderScript: " + currentVolumeValue);
        slider.value = currentVolumeValue;
        textMesh.text = currentVolumeValue.ToString();
    }

    public void OnChange()
    {
        menuManager.DetermineMenuType(menuType, slider.value, audioMenuType);
        textMesh.text = slider.value.ToString();
    }
}

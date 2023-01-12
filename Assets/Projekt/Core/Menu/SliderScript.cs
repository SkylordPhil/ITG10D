using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helper;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private MenuType menuType;
    [SerializeField] private MenuManager menuManager;
    [SerializeField] private string stringParam;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider = this.gameObject.GetComponent<Slider>(); 
    }

    public void OnChange()
    {
        menuManager.DetermineMenuType(menuType, slider.value, stringParam);
        // menuManager.sliderCHange(menuType, slider.value, stringParam)
    }
}

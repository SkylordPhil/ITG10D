using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class RebindSaveLoad : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;

    public void OnEnable()
    {
        //loads the saved rebinds from a json and adjusts the Input action asset according to the json
        if (PlayerPrefs.HasKey("rebinds"))
        {
            actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("rebinds"));
        }

        Debug.Log("Finished Loading Rebinds");
    }

    public void OnDisable()
    {
        //saves the current bindings as a json in the PlayerPrefs
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);

        Debug.Log("All Changes Saved");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;

public class LoadAndSaveJson : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset; 

    private static LoadAndSaveJson _instance;
    public static LoadAndSaveJson Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            
            _instance = value;
        }
    }
    [SerializeField] private SettingsData settingsData;
    private string path;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }
    public void Start()
    {
        GetPath();

        if (File.Exists(path))
        {
            Debug.Log("File exists");
            ReadFromFile();
        }
        else
        {
            Debug.Log("File does not Exist");
            CreateJsonFile();
        }
    }



    private void CreateJsonFile()
    {
        settingsData.SetDefaultData();
        string json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(path, json);
    }    

    private void ReadFromFile()
    {
        string json = File.ReadAllText(path);
        Debug.Log(json);
        JsonUtility.FromJsonOverwrite(json, settingsData);
        inputActionAsset.FindAction("Movement").ChangeBinding(1).Erase();
        inputActionAsset.FindAction("Movement").AddCompositeBinding("2DVector")
            .With("Up", settingsData.moveUp)
            .With("Down", settingsData.moveDown)
            .With("Left", settingsData.moveLeft)
            .With("Right", settingsData.moveRight);
        inputActionAsset.FindAction("shoot").ApplyBindingOverride(settingsData.mainAttack);
        inputActionAsset.FindAction("special").ApplyBindingOverride(settingsData.specialAbility);
    }

    private void GetPath()
    {
        path = Application.persistentDataPath + "/settings.json";
    }

    public void saveSettings()
    {
        string json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(path, json);
    }


}

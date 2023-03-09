using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAndSaveJson : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    private string path;

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
        //settingsData = JsonUtility.FromJson<SettingsData>(json);
        Debug.Log(settingsData);
    }

    private void GetPath()
    {
        path = Application.persistentDataPath + "/settings.json";
    }
}

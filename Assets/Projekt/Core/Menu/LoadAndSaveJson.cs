using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAndSaveJson : MonoBehaviour
{
    private SettingsData settingsData;
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
        settingsData = new SettingsData();
        settingsData.SetDefaultData();
        string json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(path, json);
    }

    private void ReadFromFile()
    {
        Debug.Log(path);
        string json = File.ReadAllText(path);
        Debug.Log(json);
        JsonUtility.FromJson<SettingsData>(json);
    }

    private void GetPath()
    {
        path = Application.persistentDataPath + "/settings.json";
    }
}

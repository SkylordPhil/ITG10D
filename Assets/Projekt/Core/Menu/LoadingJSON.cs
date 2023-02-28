using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadingJSON : MonoBehaviour
{
    private SettingsData settingsData;

    private string path = "";
    private string persistentPath = "";


    // Start is called before the first frame update
    void Start()
    {
        SetPaths();
        DefaultSettings();

        if (File.Exists(path))
        {
            LoadSettings();
        }
        else
        {
            SaveSettings();
        }

    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";
        Debug.Log(path);
        Debug.Log(persistentPath);
    }

    private void SaveSettings()
    {
        string savePath = path;
        Debug.Log("Saving Settings at " + path);

        string settingsJson = JsonUtility.ToJson(settingsData);
        Debug.Log(settingsJson);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(settingsJson);
    }

    private void LoadSettings()
    {
        using StreamReader reader = new StreamReader(path);
        string settingsJson = reader.ReadToEnd();

        SettingsData data = JsonUtility.FromJson<SettingsData>(settingsJson);
    }

    private void DefaultSettings()
    {
        settingsData = new SettingsData(50, 50, 50);
    }
}

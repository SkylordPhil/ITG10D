using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadAndSaveJson : MonoBehaviour
{
    private SettingsData settingsData;
    private string file;

    public void Start()
    {
        if (File.Exists(file))
        {

        }
    }

    public void LoadOrCreate()
    {
        if (File.Exists(GetFilePath(file)))
        {
            settingsData = new SettingsData();
            string json = ReadFromFile(file);
            JsonUtility.FromJsonOverwrite(json, settingsData);
        } 
        else
        {
            settingsData = new SettingsData();
            settingsData.SetSettingsData(50, 50, 50);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(settingsData);
        WriteToFile(file, json);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }

        return "";
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseSaveScript : MonoBehaviour
{

    [SerializeField] protected SettingsData settingsData;
    public void saveSettings()
    {
        string path = Application.persistentDataPath + "/settings.json";
        string json = JsonUtility.ToJson(settingsData);
        File.WriteAllText(path, json);
        Debug.Log("saved");
    }
    
}

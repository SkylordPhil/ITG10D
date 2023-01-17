using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingJSON : MonoBehaviour
{
    [SerializeField] private GameObject SettingsJson;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(SettingsJson);

    }
}

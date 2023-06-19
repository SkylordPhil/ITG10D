using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Helper;
using System;
using Newtonsoft.Json;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private List<GameObject> audioQueues;

    [SerializeField]
    private GameObject audioQueuePrefab;

    private AudioSource musicSource1;
    private AudioSource musicSource2;



    public void Awake()
    {
        SetInstance();
    }

    private void Start()
    {
        //loads the saved audio settings into the audio mixer using the audio manager
        if (PlayerPrefs.HasKey("audioVolumeValues"))
        {
            string audioValuesJson = PlayerPrefs.GetString("audioVolumeValues");

            Debug.Log(audioValuesJson);

            Debug.Log(JsonConvert.DeserializeObject<Dictionary<string, float>>(audioValuesJson));

            Dictionary<string, float> volumeValues = JsonConvert.DeserializeObject<Dictionary<string, float>>(audioValuesJson);

            foreach (KeyValuePair<string, float> volumeValue in volumeValues)
            {
                instance.SetExposedParam(volumeValue.Key, volumeValue.Value);
            }
        }
        //if the user loads the game for the first time
        else
        {
            string[] volumeNames = { "masterVolumeValue", "musicVolumeValue", "sfxVolumeValue" };

            foreach (string volumeName in volumeNames)
            {
                instance.SetExposedParam(volumeName, 50);
            }
        }
        Debug.Log(instance);
        Debug.Log("Loaded Audio Settings");
    }

    public void OnDisable()
    {
        //saves the current audio settings as a json in the PlayerPrefs
        Dictionary<string, float> volumeValues = new Dictionary<string, float>
        {
            { "masterVolumeValue", instance.GetExposedParamValue("masterVolumeValue") },
            { "musicVolumeValue", instance.GetExposedParamValue("musicVolumeValue") },
            { "sfxVolumeValue", instance.GetExposedParamValue("sfxVolumeValue") }
        };

        //Serializes the dictionary to a json
        string volumesJson = JsonConvert.SerializeObject(volumeValues, Formatting.Indented);

        PlayerPrefs.SetString("audioVolumeValues", volumesJson);

    }

    public static AudioManager GetInstance()
    {

        if (instance)
        {
            return instance;

        }

        Debug.Log("Audio Manager is not Instantiated");

        return instance;
        

    }


    public void SetInstance()
    {
        instance = this;
    }




    #region Audio Mixer


    public void SetVolume(float ln)
    {
        mixer.SetFloat("MasterVolume", MathP.ConvertLnToDB(ln));
    }

    //function that sets the value of the given mixer to the given value after converting it into decibel
    public void SetExposedParam(string paramName, float paramValue)
    {
        Debug.Log("Linear to set to: " + paramValue / 100f);
        mixer.SetFloat(paramName, MathP.ConvertLnToDB((paramValue / 100f)));
        float value;
        mixer.GetFloat(paramName, out value);
        Debug.Log("Mixer now on: " + value);
    }

    //function that returns the current value of the given mixer as a float
    public int GetExposedParamValue(string paramName) 
    {
        float value;
        mixer.GetFloat(paramName, out value);

        Debug.Log("Mixer is on: " + value + " That means as linear: " + Mathf.RoundToInt(MathP.ConvertDBToLn(value) * 100f));

        return Mathf.RoundToInt(MathP.ConvertDBToLn(value) * 100f);
    }


    #endregion 



    #region Audio SFX


    public void PlaySfxAtPosition(Transform positionTransform, AudioClip clip, AudioMixerGroup mixerGroup = null )
    {
        if(audioQueues.Count < 3)
        {
            GameObject tmp = Instantiate(audioQueuePrefab);
            audioQueues.Add(tmp);
        }

        GameObject currentObject = audioQueues[0];
        audioQueues.Remove(currentObject);
        Debug.Log(audioQueues.Count);
        currentObject.GetComponent<AudioQueue>().SetupSFX(positionTransform, clip, mixerGroup);
    }


    public void SubscribeAudioQueue(GameObject objectToSubscribe)
    {
        audioQueues.Add(objectToSubscribe);
    }


    #endregion



}

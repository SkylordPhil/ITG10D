using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Helper;



public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private List<GameObject> audioQueues;

    [SerializeField]
    private GameObject audioQueuePrefab;

    private AudioSource musicSource;



    public void Start()
    {
        SetInstance();
        musicSource = this.gameObject.GetComponent<AudioSource>();

    }
    public static AudioManager GetInstance()
    {
        if (instance)
        {
            return instance;

        }

        Debug.Log("Audio Manager is not Instatiated");

        return instance;
        

    }


    public void SetInstance()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }


    public void SetVolume(float ln)
    {
        mixer.SetFloat("MasterVolume", MathP.ConvertLnToDB(ln));
    }


    public void SetExposedParam(string paramName, float paramValue)
    {
        mixer.SetFloat(paramName , MathP.ConvertLnToDB(paramValue));
    }

    public void PlaySfxAtPosition(Transform positionTransform, AudioClip clip, AudioMixerGroup mixerGroup)
    {
        if(audioQueues.Count < 3)
        {
            GameObject tmp = Instantiate(audioQueuePrefab);
            audioQueues.Add(tmp);
        }

        GameObject currentObject = audioQueues[-1];
        audioQueues.Remove(currentObject);
        currentObject.GetComponent<AudioQueue>().SetupSFX(positionTransform, clip, mixerGroup);
    }

}

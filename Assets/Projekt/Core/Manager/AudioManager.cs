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

    private AudioSource musicSource1;
    private AudioSource musicSource2;



    public void Start()
    {
        SetInstance();

        for(int i = 0; i < 3; i++)
        {

            GameObject tmp = Instantiate(audioQueuePrefab);
            audioQueues.Add(tmp);

        }
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

    }




    #region Audio Mixer


    public void SetVolume(float ln)
    {
        mixer.SetFloat("MasterVolume", MathP.ConvertLnToDB(ln));
    }


    public void SetExposedParam(string paramName, float paramValue)
    {
        mixer.SetFloat(paramName , MathP.ConvertLnToDB(paramValue));
    }


    #endregion 



    #region Audio SFX


    public void PlaySfxAtPosition(Transform positionTransform, AudioClip clip, AudioMixerGroup mixerGroup )
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

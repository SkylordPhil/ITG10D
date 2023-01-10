using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MathP;



public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField]
    private AudioMixer mixer;


    public void Start()
    {
        setInstance();
    }
    public static AudioManager getInstance()
    {
        if (instance)
        {
            return instance;

        }

        Debug.Log("Audio Manager is not Instatiated");

        return instance;
        

    }


    public void setInstance()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }


    public void setVolume(float ln)
    {
        mixer.SetFloat("MasterVolume", MathP.MathP.ConvertLnToDB(ln));
    }


}

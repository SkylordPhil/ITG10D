using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioQueue : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void SetupSFX(Transform positionTransform, AudioClip clip, AudioMixerGroup mixerGroup)
    {

        this.transform.position = positionTransform.position;
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.Play();
        StartCoroutine(PlaySound());




    }

    public IEnumerator PlaySound() 
    {
        while(!audioSource.isPlaying)
        { 
            yield return new WaitForEndOfFrame();
        }

        AudioManager.GetInstance().SubscribeAudioQueue(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private bool isFollowing = true;
    [SerializeField] private PostProcessVolume settingsVolume;
    [SerializeField] bool isIngame;


    // Update is called once per frame
    void Update()
    {
        if(isFollowing && isIngame)
        {

            transform.Translate(((Vector2)player.transform.position - (Vector2)transform.position) * Time.deltaTime * cameraSpeed);
        
        }
        
    }


    public void EnableMenuBlur()
    {
        settingsVolume.enabled = true;
    }

    public void DisableMenuBlur()
    {
        settingsVolume.enabled = false;
    }



}

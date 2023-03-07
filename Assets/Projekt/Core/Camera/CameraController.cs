using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private bool isFollowing = true;
    [SerializeField] private PostProcessVolume settingsVolume;
    [SerializeField] bool isIngame;


    // Update is called once per frame
    void Update()
    {
        if(isFollowing && isIngame)
        {
            player = GameManagerController.Instance.getPlayer();
            transform.Translate(((Vector2)player.transform.position - (Vector2)transform.position) * Time.deltaTime * cameraSpeed);
        
        }
        
    }

    void Start()
    {
        
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

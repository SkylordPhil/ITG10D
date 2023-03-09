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
    [SerializeField] public bool isIngame;

    private static CameraController _instance;


    // Update is called once per frame
    void Update()
    {
        player = GameManagerController.Instance.getPlayer();
        if (isFollowing && isIngame && player != null)
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

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("This CameraController Was 1 too many.... Selfdestroying.....");
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }




    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("No CameraController instatiated..... maybe persistenScene is missing?");
            }
            return _instance;
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed;


    // Update is called once per frame
    void Update()
    {

        transform.Translate(((Vector2)player.transform.position - (Vector2)transform.position) * Time.deltaTime * cameraSpeed);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Translate(new Vector3(Mathf.Lerp(transform.position.x, player.transform.position.x, 0.2f ), Mathf.Lerp(transform.position.x, player.transform.position.x, 0.2f), 0).normalized * Time.deltaTime);

        transform.Translate(((Vector2)player.transform.position - (Vector2)transform.position) * Time.deltaTime * cameraSpeed);
        
    }
}

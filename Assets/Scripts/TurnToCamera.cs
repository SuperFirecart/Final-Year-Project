using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToCamera : MonoBehaviour
{
    private GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = playerCamera.transform.rotation;
    }
}

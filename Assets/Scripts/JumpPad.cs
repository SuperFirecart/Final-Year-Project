using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JumpPad : MonoBehaviour
{
    public float launchPower;
    public float aimedHeight;
    private float inclineAngle;
    private Vector3 launchVector = Vector3.zero;
    
    void Start()
    {
        if (aimedHeight == 0 && launchPower != 0){
            aimedHeight = launchPower - 3;
        }
        else if (aimedHeight != 0 && launchPower == 0) {
            launchPower = workOutPower(aimedHeight + 2);
        }
        else {
            print("empty launch pad");
        }

        launchVector = new Vector3(transform.up.x * launchPower, transform.up.y * launchPower, transform.up.z * launchPower) * 3f;

    }
    private float workOutPower(float aimedHeight){
        return aimedHeight + 6;
    }

    private void OnTriggerStay(Collider other)
    {
        //launch power is in the ratio of (0.025(LaunchPower)^2) + 1
        // launch the entity that enters this space
        if (other.tag == "Player" || other.tag == "Enemy"){
            Movement moveScript = GameObject.Find(other.gameObject.name).GetComponent<Movement>();
            moveScript.setVectorSpeed(launchVector);
        }
        else {
            print("Other");
        }
    }
}

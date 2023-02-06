using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement: MonoBehaviour {
    private float moveSpeed = 10f;
    private float jumpSpeed = 8f;
    private float dashSpeed = 10f;
    private float gravity = 20f;

    public int numDashes = 0;
    public int dashDelay = 0;
    public int maxDashes = 1;
    public int numJumps = 0;
    public int maxJumps = 1;
    public int jumpDelay = 0;
    public int maxDelay = 1;
    
    private Vector3 relDirection = Vector3.zero;
    private Vector3 extraDire = Vector3.zero;
    private CharacterController chara;
    public bool jump = false;
    public bool dash = false;

    
    public void Start() {
        chara = GetComponent<CharacterController>();
    }

    public void move(Vector3 moveDirection) {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);
        relDirection = (forward * relDirection.x) + (up * relDirection.y) + (right * relDirection.z);
        if (jump) {
            if (extraDire.y <= 2f){
                extraDire.y = 0f;
            }
            if (relDirection.y >= 5f) {
                relDirection.y += jumpSpeed * 0.4f;
            }
            else if(relDirection.y >= 0) {
                relDirection.y += jumpSpeed;
            }
            else {
                relDirection.y = jumpSpeed;
            }
            jump = false;
        }

        if(dash){
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                extraDire = (forward * Input.GetAxis("Vertical") * dashSpeed) + (up * relDirection.y) + (right * Input.GetAxis("Horizontal") * dashSpeed);
            }
            else
            {
                extraDire = forward * dashSpeed;
            }
            dash = false;
        }

        if (!chara.isGrounded){
            relDirection.y -= gravity * Time.deltaTime;
        }
        else
        {
            if (relDirection.y < 0){
                relDirection.y = 0;
            }
        }

        // Friction
        if (moveDirection.y >= 5f) {
            moveDirection.y /= 1.05f;
        }
        moveDirection.x /= 1.2f;
        moveDirection.z /= 1.2f;
        
        
        chara.Move(new Vector3((moveDirection.x + relDirection.x) * moveSpeed, moveDirection.y + relDirection.y, (moveDirection.z + relDirection.z) * moveSpeed) * Time.deltaTime);
        
        if (extraDire != Vector3.zero){
            chara.Move(extraDire * Time.deltaTime); 
            if(!chara.isGrounded){
                // if (extraDire.y >= 0f){
                //     extraDire.y -= (gravity * 0.25f) * Time.deltaTime;
                // }
                extraDire /= 1.05f;
            }
            else{
                extraDire.y = 0f;
                extraDire.x /= 1.5f;
                extraDire.z /= 1.5f;
            }
        }
    }

    public void setVectorSpeed(Vector3 extra){
        extraDire = extra;
        relDirection.y = 0f;
        numDashes = 0;
        numJumps = 0;
    }

    public void setMaxJumps(int newJumps){
        maxJumps = newJumps;
    }
    
    public void setMaxDashes(int newDashes){
        maxDashes = newDashes;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Movement: MonoBehaviour {
    private float moveSpeed = 10f;
    private float jumpSpeed = 15f;
    private float dashSpeed = 100f;
    private float gravity = 30f;

    public int numDashes = 0;
    public int dashDelay = 0;
    public int maxDashes = 1;
    public int numJumps = 0;
    public int maxJumps = 1;
    public int jumpDelay = 0;
    public int maxDelay = 1;

    private Vector3 checkpoint = new Vector3(18, 2, -12);
    
    private Vector3 relDirection = Vector3.zero;
    private Vector3 extraDire = Vector3.zero;
    private CharacterController chara;
    public bool jump = false;
    public bool dash = false;
    public GameObject radial;
    public GameObject vertical;

    public bool restart = false;


    
    public void Start() {
        chara = GetComponent<CharacterController>();
        radial = GameObject.Find("radialLines");
        radial.SetActive(false);
        vertical = GameObject.Find("verticalLines");
        vertical.SetActive(false);
    }

    public void move(Vector3 moveDirection) {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);
        relDirection = (forward * relDirection.x) + (up * relDirection.y) + (right * relDirection.z);
        if (jump) {
            // if (extraDire.y <= 2f){
            //     extraDire.y = 0f;
            // }
            if (relDirection.y >= 5f) {
                relDirection.y += jumpSpeed * 0.4f;
            }
            else if(relDirection.y >= 0) {
                relDirection.y += jumpSpeed;
            }
            else {
                relDirection.y = jumpSpeed;
            }
            vertical.SetActive(false);
            jump = false;
        }

        if(dash){
            //play the radial lines video
            StartCoroutine(ShowRadial(0.2f));
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                // relDirection += (forward * Input.GetAxis("Vertical") * dashSpeed) + (right * Input.GetAxis("Horizontal") * dashSpeed);
                extraDire += ((forward * Input.GetAxis("Vertical") * dashSpeed) + (right * Input.GetAxis("Horizontal") * dashSpeed));
            }
            else
            {
                extraDire += forward * dashSpeed;
            }
            dash = false;
        }

        if (!chara.isGrounded){
            relDirection.y -= gravity * Time.deltaTime;
            if (relDirection.y < -10f){
                //play the vertical motion lines video
                vertical.SetActive(true);
                vertical.transform.GetComponent<VideoPlayer>().playbackSpeed = (-1*relDirection.y) / 15f;

            }
        }
        else
        {
            vertical.SetActive(false);
            relDirection.y -= (gravity/2) * Time.deltaTime;
            if (relDirection.y < 0f){
                relDirection.y = 0f;
            }
            
            numDashes = 0;
            numJumps = 0;
            extraDire.y = 0f;
            extraDire.x /= 1.5f;
            extraDire.z /= 1.5f;
        }
        relDirection.x /= 1.5f;
        relDirection.z /= 1.5f;

        // Friction
        if (moveDirection.y >= 5f) {
            moveDirection.y /= 1.05f;
        }
        moveDirection.x /= 1.5f;
        moveDirection.z /= 1.5f;
        
        
        chara.Move(new Vector3((moveDirection.x + relDirection.x) * moveSpeed, moveDirection.y + relDirection.y, (moveDirection.z + relDirection.z) * moveSpeed) * Time.deltaTime);
        
        if (extraDire != Vector3.zero){
            chara.Move(extraDire * Time.deltaTime); 
            if(!chara.isGrounded){
                // if (extraDire.y >= 0f){
                //     extraDire.y -= (gravity * 0.25f) * Time.deltaTime;
                // }
                extraDire /= 1.3f;
            }
            else{
                numDashes = 0;
                numJumps = 0;
                extraDire.y = 0f;
                extraDire.x /= 1.5f;
                extraDire.z /= 1.5f;
            }
        }
        else{
            if(chara.isGrounded){
                numDashes = 0;
                numJumps = 0;
            }
        }
        
        if (chara.transform.position.y <= -20 || restart == true){
            chara.transform.position = checkpoint;
            restart = false;
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

    public void setCheckpoint(Vector3 pos){
        checkpoint = pos;
    }
    public Vector3 getCheckpoint(){
        return checkpoint;
    }

    IEnumerator ShowRadial(float delay)
    {
    radial.SetActive(true);
    yield return new WaitForSeconds(delay);
    radial.SetActive(false);
    }

}
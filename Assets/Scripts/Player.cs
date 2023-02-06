using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector3 direction;
    // private float dashSpeed = 100f;
    public int health = 100;
    public int damagePower = 5;
    public int maxHealth = 100;
    public float Invulnerability = 25f;
    public Camera playerCamera;
    public float lookSpeed = 20.0f;
    float rotationX = 0;
    public float lookXLimit = 180.0f;
    public string attackType = "Rapid";

    public GameObject Bullet;
    private Movement moveScript;



    List<float> highestValue;


    

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(0,0,0);
        characterController = GetComponent<CharacterController>();
        // Cursor.lockState = CursorLockMode.Locked; // lock the cursor into the screen      
        moveScript = GetComponent<Movement>();
        highestValue = new List<float>() {transform.position.y};
        moveScript.setMaxJumps(3);
        moveScript.setMaxDashes(3);
        setDisplay();  
    }

    // Update is called once per frame
    void Update()
    {   
        //I-frames to avoid damage
        if(Invulnerability > 0){
            Invulnerability -= 1f;
        }
        //relative forwards, right, and up
        
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 up = transform.TransformDirection(Vector3.up);
        
        direction.x = Input.GetAxis("Vertical");
        direction.z = Input.GetAxis("Horizontal");

        direction = (forward * direction.x) + (up * direction.y) + (right * direction.z);

        if (!characterController.isGrounded) {
        //     direction.y -= gravity * Time.deltaTime;
            // highestValue.AddRange(new List<float>{transform.position.y - 1.08f});
        }
        else {
            if(moveScript.jumpDelay >= 1 && moveScript.dashDelay >= 1) {
                moveScript.numJumps = 0;
                moveScript.numDashes = 0;
                setDisplay();
            }

            // if (highestValue.Max() != 0f){
            //     print(highestValue.Max());
            // }
            // highestValue = new List<float> {0f};
        }
        
        
        if (Input.GetButtonDown("Jump") && (moveScript.numJumps < moveScript.maxJumps) && (moveScript.jumpDelay >= moveScript.maxDelay))
        {
            moveScript.jump = true;

            moveScript.jumpDelay = 0;
            moveScript.numJumps += 1;
            setDisplay();
        }
        moveScript.jumpDelay += 1;
        moveScript.dashDelay += 1;

        
        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // if(Input.GetButtonDown("Fire1")){
        //     Transform fireDirection = playerCamera.transform;
        //     attack.shootAttack(attackType, Bullet, damagePower, fireDirection);
        // }
        // ######
        if(Input.GetKeyDown(KeyCode.V)){
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene("Main Menu");
        }
        // // ######
        if (Input.GetKeyDown(KeyCode.Q) && (moveScript.numDashes < moveScript.maxDashes) && (moveScript.dashDelay >= moveScript.maxDelay))
        {
            moveScript.dash = true;

            moveScript.dashDelay = 0;
            moveScript.numDashes += 1;
            setDisplay();
        }   
    }
    // physics stuff
    void FixedUpdate(){
        moveScript.move(direction);
    }


    public void setDisplay()
    {
        GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text = "Health: " + health + " / " + maxHealth;
        GameObject.Find("Jumps Dashes Text").GetComponent<TextMeshProUGUI>().text = "Jumps: " + (moveScript.maxJumps - moveScript.numJumps) + " / " + moveScript.maxJumps + "\nDashes: " + (moveScript.maxDashes - moveScript.numDashes) + " / " + moveScript.maxDashes + "\nDamage: " + damagePower;
    }

    public void takeDamage(int damage) {
        health -= damage;
        if (health <= 0)
        {
            health = 100;
        }
        GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text =  "Health: " + health + " / " + maxHealth;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Pickups
        // "HealthRestore", "HealthUp", "DamageUp", "DashesIncrease", "JumpsIncrease"
        switch (other.tag){
            case "HealthRestore":
                if (health == maxHealth){
                    print("no health restored as already at max health");
                }
                else{
                    if (health + 10 >= maxHealth){
                        health = maxHealth;
                    }
                    else {
                        health += 10;
                    }
                Destroy(other.gameObject);
                }
                break;
            case "HealthUp":
                maxHealth += 10;
                health += 10;
                Destroy(other.gameObject);
                break;
            case "DamageUp":
                damagePower += 10;
                Destroy(other.gameObject);
                break;
            case "DashesUp":
                moveScript.maxDashes += 1;
                Destroy(other.gameObject);
                break;
            case "JumpsUp":
                moveScript.maxJumps += 1;
                Destroy(other.gameObject);
                break;
        }
        setDisplay();
    }

}


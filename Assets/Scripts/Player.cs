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
    public int health = 3;
    public int damagePower = 5;
    private int CheckpointsCollected = 0;
    public float Invulnerability = 25f;
    public Camera playerCamera;
    public float lookSpeed = 20.0f;
    float rotationX = 0;
    public float lookXLimit = 180.0f;
    public string attackType = "Rapid";

    public GameObject Bullet;
    private Movement moveScript;
    private AStar pathing;



    List<float> highestValue;
    private float timer;
    public Material mat;

    private GameObject deathMessage;
    private GameObject toggle;


    

    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(0,0,0);
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor into the screen      
        moveScript = GetComponent<Movement>();
        pathing = GetComponent<AStar>();
        toggle = GameObject.Find("PathToggle");
        highestValue = new List<float>() {transform.position.y};
        moveScript.setMaxJumps(2);
        moveScript.setMaxDashes(1);
        timer = 0.0f;
        deathMessage = GameObject.Find("DeathMessage");
        deathMessage.SetActive(false);
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
        timer += Time.deltaTime;
        
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && (moveScript.numDashes < moveScript.maxDashes) && (moveScript.dashDelay >= moveScript.maxDelay))
        {
            moveScript.dash = true;

            moveScript.dashDelay = 0;
            moveScript.numDashes += 1;
            setDisplay();
        }
        moveScript.jumpDelay += 1;
        moveScript.dashDelay += 1;

        
        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        setTimer();

        // if(Input.GetButtonDown("Fire1")){
        //     Transform fireDirection = playerCamera.transform;
        //     attack.shootAttack(attackType, Bullet, damagePower, fireDirection);
        // }
        // ######
        if(Input.GetKeyDown(KeyCode.T)){
            if (!pathing.showPath){
                toggle.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
                pathing.showOptimal();
            }
            else{
                toggle.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
                pathing.showRegular();
            }
            pathing.showPath = !pathing.showPath;
        }
        // // ######
    }
    // physics stuff
    void FixedUpdate(){
        moveScript.move(direction);
    }

    void FinishGame(int statis){
        scores.score = statis;
        if (statis == 1){
            scores.timeTaken = (int)timer;
        }
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }


    public void setDisplay()
    {
        GameObject.Find("Lives").GetComponent<TextMeshProUGUI>().text = "Lives: " + health;

        GameObject.Find("Jumps Dashes Text").GetComponent<TextMeshProUGUI>().text = "Jumps: " + (moveScript.maxJumps - moveScript.numJumps) + " / " + moveScript.maxJumps + "\nDashes: " + (moveScript.maxDashes - moveScript.numDashes) + " / " + moveScript.maxDashes;
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + CheckpointsCollected;
    }

    public void setTimer(){
        GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>().text = "Time Survived: " + timer.ToString("#.##");
    }

    // public void takeDamage(int damage) {
    //     health -= damage;
    //     if (health <= 0)
    //     {
    //         health = maxHealth;
    //     }
    //     GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>().text =  "Health: " + health + " / " + maxHealth;
    // }
    private void OnTriggerEnter(Collider other)
    {
        // Pickups
        // "HealthRestore", "HealthUp", "DamageUp", "DashesIncrease", "JumpsIncrease"
        switch (other.tag){
            // case "HealthRestore":
                // if (health == maxHealth){
                //     print("no health restored as already at max health");
                // }
                // else{
                //     if (health + 10 >= maxHealth){
                //         health = maxHealth;
                //     }
                //     else {
                //         health += 10;
                //     }
                // Destroy(other.gameObject);
                // }
                // break;
            case "CheckPoint":
                CheckpointsCollected += 1;
                if (CheckpointsCollected == 3) {//show congratulations screen
                    FinishGame(1);
                }
                health += 1;
                moveScript.setCheckpoint(other.gameObject.transform.position);
                AStar pathing = GetComponent<AStar>();
                pathing.platforms.outputList[0].GetComponent<Renderer>().material = mat;
                pathing.platforms.outputList[pathing.platforms.outputList.Count-1].GetComponent<Renderer>().material = mat;
            
                //generate new path to one of the other platforms
                Destroy(GameObject.Find("ParkourGroup"));
                Destroy(other.gameObject);
                pathing.GoAgain();
                GetComponent<EvilGrass>().StopAttack();
                GetComponent<EvilGrass>().grassAttackSetup();

                break;
            // case "HealthUp":
            //     maxHealth += 10;
            //     health += 10;
            //     Destroy(other.gameObject);
            //     break;
            // case "DamageUp":
            //     damagePower += 10;
            //     Destroy(other.gameObject);
            //     break;
            case "DashesUp":
                moveScript.maxDashes += 1;
                Destroy(other.gameObject);
                break;
            case "JumpsUp":
                moveScript.maxJumps += 1;
                Destroy(other.gameObject);
                break;
            case "Grass":
                // minus 1 to life, teleport to last checkpoint
                if (health != 0) {
                    health -= 1;
                }
                else{
                    moveScript.vertical.SetActive(false);
                    moveScript.radial.SetActive(false);
                    GetComponent<EvilGrass>().StopAttack();
                    GetComponent<EvilGrass>().enabled = false;
                    enabled = false;
                    FinishGame(2);
                }
                moveScript.restart = true;
                break;

        }
        setDisplay();
    }
    public int getCheckpointsCollected(){
        return CheckpointsCollected;
    }
}


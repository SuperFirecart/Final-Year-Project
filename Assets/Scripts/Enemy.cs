using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 10f;
    public int damage = 5;
    private Vector3 direction;
    private float startPos;
    private Transform healthText;
    
    public Vector3 moveDirection = Vector3.zero;
    private CharacterController chara;
    private Movement move;

    //Moves this GameObject 2 units a second in the forward direction
    void Start()
    {
        direction = new Vector3(0,0,0);
        startPos = transform.position.x;
        healthText = this.gameObject.transform.GetChild(0);
        setHealthText();
        chara = GetComponent<CharacterController>();
        move = GetComponent<Movement>();
        moveDirection = Vector3.zero;
    }

    void FixedUpdate()
    {   
        // move.move(moveDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            Player PlayerScript = GameObject.Find(other.gameObject.name).GetComponent<Player>();
            if (PlayerScript.Invulnerability <= 0)
            {
                if (PlayerScript.health <= 0)
                {
                    /*Destroy(other.gameObject, 0f);*/
                    PlayerScript.takeDamage(-20);
                    PlayerScript.Invulnerability = 25f;

                }
                else
                {
                    PlayerScript.takeDamage(damage);
                    PlayerScript.Invulnerability = 25f;
                }
            }
        }
    }

    public void setHealthText() {
        healthText.gameObject.GetComponent<TMPro.TextMeshPro>().text = "Health: " + health;
    }
}

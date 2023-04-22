using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityText : MonoBehaviour
{
    private GameObject player;
    private Transform speakingText;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        speakingText = transform.GetChild(0);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 10){
            //switch case for the destination texts
            // print(player.GetComponent<AStar>().objective.transform.position);
            if (player.GetComponent<AStar>().objective.transform.position == new Vector3(-65f, 30f, -45f)){
                speakingText.gameObject.GetComponent<TMPro.TextMeshPro>().text = "To the West Wing";
            }
            else if (player.GetComponent<AStar>().objective.transform.position == new Vector3(98f, 28f, -65f)){
                speakingText.gameObject.GetComponent<TMPro.TextMeshPro>().text = "Cross the Quad";
            }
            else{
                speakingText.gameObject.GetComponent<TMPro.TextMeshPro>().text = "To the Top of the North Tower";
            }

        }
        else{
            transform.position = new Vector3(player.GetComponent<AStar>().objective.transform.position.x + 3, player.GetComponent<AStar>().objective.transform.position.y + 0.5f, player.GetComponent<AStar>().objective.transform.position.z + 3);

            //set text to blank
            speakingText.gameObject.GetComponent<TMPro.TextMeshPro>().text = "";
        }
    }
}
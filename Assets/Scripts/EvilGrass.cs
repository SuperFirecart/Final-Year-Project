using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EvilGrass : MonoBehaviour
{   
    public Material mat;

    private List<GameObject> grasses = new List<GameObject>();
    private System.Random rnd;
    private AStar pathing;
    private Player player;
    private GameObject grass1;
    private Renderer grassRender;
    private bool oneGrass = false;
    private List<int> attackList;
    private IEnumerator attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {   
        grass1 = GameObject.Find("Grass1");
        grasses.Add(grass1);
        grassRender = grass1.GetComponent<Renderer>();
        grasses.Add(GameObject.Find("Grass2"));
        grasses.Add(GameObject.Find("Grass3"));
        grasses.Add(GameObject.Find("Grass4"));
        pathing = GetComponent<AStar>();
        player = GetComponent<Player>();
        rnd = new System.Random();
        grassAttackSetup();
    }

    void Update(){
        if (grasses.Count != 1){
            for (int i = 0; i < grasses.Count; i ++){
                grasses[i].gameObject.transform.localScale += new Vector3(0.5f, 0f, 0.5f) * Time.deltaTime;
                if (grasses[i].gameObject.transform.localScale.x >= 22f && grasses[i].gameObject.transform.localScale.z >= 22f){
                    if (!oneGrass){
                        moveGrass();
                        oneGrass = true;
                    }
                    if (i != 0){
                        Destroy(grasses[i]);
                        grasses.RemoveAt(i);
                    }
                    
                }
                // grasses[i].gameObject.transform.localScale.z += player.getCheckpointsCollected() * Time.deltaTime;
                // grasses[i].gameObject.transform.localScale.y += (player.getCheckpointsCollected() * Time.deltaTime) * 0.2;
            }
        }
        else if (grasses.Count == 1){
            grass1.gameObject.transform.localScale += new Vector3(0.75f, 0f, 0.75f) * Time.deltaTime;
            float scaleX = (Time.time * 0.1f) + 9f;
            float scaleY = (Time.time * 0.1f) + 9f;
            grassRender.material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
        }
    }

    void moveGrass(){
        grass1.gameObject.transform.position = new Vector3(18f, -0.65f, -12f);
        grass1.gameObject.transform.localScale = new Vector3(42.5f, 1f, 42.5f);
    }

    public void grassAttackSetup(){
        attackCoroutine = Attack();
        StartCoroutine(attackCoroutine);
    }

    IEnumerator Attack(){
        while (true){
            yield return new WaitForSeconds(3);
            attackList = new List<int>();
            int nextShot = 0;
            while (nextShot == 0){
            nextShot = rnd.Next(1, pathing.platforms.outputList.Count-1);
                if (!attackList.Contains(nextShot)){
                    attackList.Add(nextShot);
                }
                else {
                    nextShot = 0;
                }
            }
            FireGrass(pathing.platforms.outputList[nextShot]);
            // recalculate path after removing the platform from possible list
            // print("rerunning AStar");
            pathing.platforms.outputList.RemoveAt(nextShot);
            pathing.generatePath();
        }
        // print(attackList[bullet]);
    }
    public void StopAttack(){
        StopCoroutine(attackCoroutine);
    }

    public void FireGrass(GameObject plat){
        attack grassAtt = grass1.GetComponent<attack>();
        if (grassAtt != null){
            grassAtt.shootAttack(grass1.transform.position, plat);
        }
    }
}

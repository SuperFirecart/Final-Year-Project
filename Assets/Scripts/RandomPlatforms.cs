using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomPlatforms : MonoBehaviour
{
    private int length;
    private int height;
    private int width;


    private float xdir;
    private float ydir;
    private float zdir;


    public int odds = 1;
    private GameObject parkourGroup;
    public Material mat;
    public List<GameObject> outputList;
    private AStar pathing;
    // public Octree<GameObject> boundsTree = new Octree<GameObject>(3f, new Vector3(7.5f, 7.5f, 7.5f), 1f, 1.25f);
    // void Start()
    // {
    //     // Octree<GameObject> boundsTree = new Octree<GameObject>(15, new Vector3(0,0,0), 1, 1.25f);
    //     // createPlatformData();
    // }
    // void OnDrawGizmos() {
    //     boundsTree.DrawAllBounds(); // Draw node boundaries
    //     boundsTree.DrawAllObjects(); // Draw object boundaries
    //     boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries
    // }
    
    public void createPlatformData(GameObject startNode, Vector3 direction){
        // length = ((int)direction.x - (int)startNode.transform.position.x) + 5;
        // height = ((int)direction.y - (int)startNode.transform.position.y) + 5;
        // width =  ((int)direction.z - (int)startNode.transform.position.z) + 5;
        length = 6;
        height = 6;
        width = (int)(MathF.Sqrt(MathF.Pow(direction.x, 2) + MathF.Pow(direction.z, 2)))/5;
        parkourGroup = new GameObject();
        parkourGroup.transform.position = startNode.transform.position;
        parkourGroup.name = "ParkourGroup";
        pathing = GetComponent<AStar>();

        System.Random rnd = new System.Random();
        List<List<List<int>>> PlatformList = new List<List<List<int>>>();
        for (int i = 0; i < length; i ++){
            List<List<int>> temperlist = new List<List<int>>();
            for (int j = 0; j < height; j ++){
                List<int> templist = new List<int>();
                for (int k = 0; k < width; k ++){
                    templist.Add(0);
                }
                temperlist.Add(templist);
            }
            PlatformList.Add(temperlist);
        }

        Vector3 angle = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
        Vector3 spawn = parkourGroup.transform.position;
        outputList = new List<GameObject>();
        outputList.Add(startNode);
        for(int x = -PlatformList.Count/2; x < PlatformList.Count/2; x++){
            for(int y = 0; y < PlatformList[0].Count; y++){
                for(int z = 0; z < PlatformList[0][0].Count; z++){

                        int xRot = rnd.Next(0, 12);
                        int yRot = rnd.Next(0, 12);
                        int zRot = rnd.Next(0, 12);

                        int rndX = rnd.Next(-2, 3);
                        int rndZ = rnd.Next(-2, 3);
                        
                        if(rnd.Next(0,odds + 1) == 1){
                            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            platform.transform.parent = parkourGroup.transform;
                            platform.transform.position = new Vector3(spawn.x + (x * 7f) + rndX, spawn.y + (y * 7f) - 10f, spawn.z + (z * 7f) + rndZ);
                            // platform.transform.position = startNode.transform.position + direction;
                            platform.transform.localScale = new Vector3(3f, 1f, 3f);
                            // platform.transform.Rotate(Quaternion.LookRotation(direction, Vector3.up).eulerAngles);
                            platform.transform.Rotate(new Vector3((xRot * 30f), (yRot*30f), (zRot*30f)));
                            platform.GetComponent<Renderer>().material = mat;
                            outputList.Add(platform);

                            // Bounds boundingBox = platform.GetComponent<Renderer>().bounds;
                            
                            // Bounds myBounds = new Bounds(new Vector3((x * 6f), (y * 5f) + 5f + 1f, (z * 6f)), new Vector3(3f, 1f, 3f));
                            // boundsTree.Add(platform, boundingBox);
                        // }
                    }
                }
            }
        }
        parkourGroup.transform.position = startNode.transform.position;
        parkourGroup.transform.Rotate(new Vector3(angle.x, angle.y, 0));
        for (int i = 0; i < outputList.Count; i ++){// Clear platforms in the aula maxima
            if (!CollideDetect(outputList[i].transform.position)){
                Destroy(outputList[i]);
                outputList.RemoveAt(i);
            }
        }
        // GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // platform.transform.position = new Vector3(xdir + (0f * 7f), ydir + (1f * 5f) + 5f, zdir + (0f * 7f));
        
    }
    Boolean CollideDetect(Vector3 pos){
        if (pos.y < 0f){
            return false;
        }

        for (int i = 0; i < pathing.possibleObjectives.Count; i ++) {
            Transform curObj = pathing.possibleObjectives[i].transform;
            if (pos.x <= curObj.position.x + curObj.localScale.x && pos.x >= curObj.position.x - curObj.localScale.x){
                if (pos.y <= curObj.position.y + 10f && pos.y >= curObj.position.y + 1f){
                    if (pos.z <= curObj.position.z + curObj.localScale.z && pos.z >= curObj.position.z - curObj.localScale.z){
                        return false;
                    }
                }
            }
        }

        if (pos.x <= -50f && pos.x >= -75f){//west wing
            if (pos.y <= 25f){
                if (pos.z < 70f && pos.z >= -50f){
                    return false;
                }
            }
        }
        if (pos.x <= 110f && pos.x >= 82f){//east wing
            if (pos.y <= 25f){
                if (pos.z < 70f && pos.z >= -70f){
                    return false;
                }
            }
        }
        if (pos.x <= -25f && pos.x >= 0f){//tower
            if (pos.y <= 40f){
                if (pos.z < 72f && pos.z >= 50f){
                    return false;
                }
            }
        }
        if (pos.x <= 115f && pos.x >= 15f){//Aula Maxima
            if (pos.y <= 25f){
                if (pos.z < 108f && pos.z >= 81f){
                    return false;
                }
            }
        }
        if (pos.x <= 106f && pos.x >= -73f){//back row
            if (pos.y <= 25f){
                if (pos.z < 71f && pos.z >= 50f){
                    return false;
                }
            }
        }
        return true;

        
        
    }
}



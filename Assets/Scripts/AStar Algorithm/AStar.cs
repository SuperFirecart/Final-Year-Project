using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class AStar : MonoBehaviour
{
    public GameObject startNode;
    public GameObject objective;
    public GameObject pickUpCheckpoint;
    public List<GameObject> possibleObjectives;
    public Vector3 direction;
    public RandomPlatforms platforms;
    public List<AStarEdge> edges;
    public Material mat;
    public Material defaultMat;
    private int currentCheckPoint = 0;
    private int nextCheckPoint = 1;
    private System.Random rnd;
    public bool showPath = false;
    private List<GameObject> optimalPath;

    // Start is called before the first frame update
    void Start()
    {//populate possible platform
        rnd = new System.Random();
        possibleObjectives.Add(GameObject.Find("Checkpoint1"));
        possibleObjectives.Add(GameObject.Find("Checkpoint2"));
        possibleObjectives.Add(GameObject.Find("Checkpoint3"));
        possibleObjectives.Add(GameObject.Find("Checkpoint4"));
        possibleObjectives.Add(GameObject.Find("Checkpoint5"));
        setup();
    }
    public void setup(){
        Player play = GetComponent<Player>();
        if (play.getCheckpointsCollected() <= 3){
            currentCheckPoint = play.getCheckpointsCollected();
            nextCheckPoint = currentCheckPoint + 1;
        }
        else{
            currentCheckPoint = nextCheckPoint;
            nextCheckPoint = rnd.Next(0,4);    
        }
        startNode = possibleObjectives[currentCheckPoint];
        objective = possibleObjectives[nextCheckPoint];

        //generate the item pickup for the objective
        GameObject pickup = Instantiate(pickUpCheckpoint, new Vector3(objective.transform.position.x,objective.transform.position.y + 2,objective.transform.position.z), Quaternion.identity);
        pickup.transform.SetParent(objective.transform);
        
        direction = new Vector3(objective.transform.position.x - startNode.transform.position.x, objective.transform.position.y - startNode.transform.position.y, objective.transform.position.z - startNode.transform.position.z);
        // print(direction);
        GetComponent<RandomPlatforms>().createPlatformData(startNode, direction);
        // Octree<GameObject> boundaries = platforms.GetComponent<RandomPlatforms>().boundsTree;
        platforms.outputList.Add(objective);
        generatePath();
    }
    public void generatePath(){
        edges = new List<AStarEdge>();
        for (int i = 0; i < platforms.outputList.Count; i ++){
            for (int j = i+1; j < platforms.outputList.Count; j ++) {
                float distance = Vector3.Distance(platforms.outputList[i].transform.position, platforms.outputList[j].transform.position);
                if (withinDistance(platforms.outputList[i].transform.position, platforms.outputList[j].transform.position)){
                    // print(distance);
                    AStarEdge edge = new AStarEdge{fromPlatform = platforms.outputList[i], toPlatform = platforms.outputList[j], distance = distance};
                    edges.Add(edge);
                }
            }
        }
        // print(edges[1].distance + " " + edges[1].fromPlatform.transform.position + " " + edges[1].toPlatform.transform.position);
        optimalPath = FindPath(startNode, objective);
        showRegular();
        if (optimalPath != null){
            if (showPath){
                showOptimal();
            }
        }
        else{
            //generate another platform between furthest node and destination
            print("1");
            
            // platforms.Destroy();
            // setup();
        }
        // print(optimalPath);
        
    }
    public void showOptimal(){
        for (int i = 0; i < optimalPath.Count; i++){
            optimalPath[i].GetComponent<Renderer>().material = mat;
        }
    }

    public void showRegular(){
        for (int i = 1; i < platforms.outputList.Count-1; i++){
            platforms.outputList[i].GetComponent<Renderer>().material = defaultMat;
        }
        platforms.outputList[0].GetComponent<Renderer>().material = GameObject.Find("Player").GetComponent<Player>().mat;
        platforms.outputList[platforms.outputList.Count-1].GetComponent<Renderer>().material = GameObject.Find("Player").GetComponent<Player>().mat;
    }

    public List<GameObject> FindPath(GameObject startNode, GameObject objective){
        List<GameObject> openList = new List<GameObject>{startNode};
        Dictionary<GameObject, GameObject> backtracker = new Dictionary<GameObject, GameObject>();
        Dictionary<GameObject, float> neighbouringdistances = new Dictionary<GameObject, float>{{startNode, 0f}};
        Dictionary<GameObject, float> globalDistances = new Dictionary<GameObject, float>{{startNode, Vector3.Distance(startNode.transform.position, objective.transform.position)}};

        while(openList.Count > 0){
            GameObject current = openList.OrderBy(platform => globalDistances[platform]).First();
            if (current.transform.position == objective.transform.position){
                List<GameObject> path = new List<GameObject>{objective};
                while (backtracker.ContainsKey(path[0])){
                    path.Insert(0, backtracker[path[0]]);
                }
                return path;
            }
            openList.Remove(current);

            foreach (AStarEdge edge in from e in edges where e.fromPlatform == current select e) {
                GameObject neighbor = edge.toPlatform;
                float tentativeGScore = neighbouringdistances[current] + edge.distance;

                if (!neighbouringdistances.ContainsKey(neighbor) || tentativeGScore < neighbouringdistances[neighbor]) {
                    backtracker[neighbor] = current;
                    neighbouringdistances[neighbor] = tentativeGScore;
                    globalDistances[neighbor] = neighbouringdistances[neighbor] + Vector3.Distance(neighbor.transform.position, objective.transform.position);

                    if (!openList.Contains(neighbor)) {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        //Create new platform between closest platform and final platform

        //rerun pathing

        // No path found
        return null;
    }

    public void GoAgain(){
        setup();
        // print("again");
    }
    

    public bool withinDistance(Vector3 platformA, Vector3 platformB){
        float maxDistance = 20f;
        float yDifference = MathF.Abs(platformA.y - platformB.y);
        if (platformA.y > platformB.y){
            maxDistance += MathF.Pow(1.1f, yDifference);
        }
        else {
            maxDistance -= yDifference/4;
            if (maxDistance < 0) {
                maxDistance = 0;
            }
        }
        bool isWithinDistance = (platformA.y > platformB.y && Vector3.Distance(platformA, platformB) <= maxDistance) || (yDifference <= 9f && Vector3.Distance(platformA, platformB) <= maxDistance);
        return isWithinDistance;
    }
}

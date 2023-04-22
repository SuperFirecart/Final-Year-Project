using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Text.Json;

public class MenuButtons : MonoBehaviour
{
    private GameObject playGameButton;
    private GameObject nameField;
    private GameObject scoreBoard;
    private GameObject outputField;
    // Start is called before the first frame update
    void Start()
    {
        outputField = GameObject.Find("OutputField");
        int statis = scores.score;
        if (statis == 1){
            //display congratulations
            outputField.GetComponent<TextMeshProUGUI>().text = "Congratulations!";
            AddToJson();
            scores.name = "";
            scores.score = 0;
            scores.timeTaken = 0;
        }
        else if (statis == 2){
            outputField.GetComponent<TextMeshProUGUI>().text = "Better Luck Next Time";
        }
        else{
            outputField.GetComponent<TextMeshProUGUI>().text = "Welcome to Quad Jump!";
        }
        ReadScoreBoard();
        
        playGameButton = GameObject.Find("PlayGame").gameObject;
        nameField = GameObject.Find("NameField").gameObject;
        // scoreBoard = GameObject.Find("scoreBoard").gameObject;
        // List<Item> items = ReadScoreBoard();
        playGameButton.GetComponent<Button>().onClick.AddListener(PlayOnClick);
    }

    void PlayOnClick(){
        if (nameField.GetComponent<TMP_InputField>().text != ""){
            AudioSource audioData = GetComponent<AudioSource>();
            audioData.Pause();
            scores.name = nameField.GetComponent<TMP_InputField>().text;
            // scores.name 
            SceneManager.LoadScene("Pathing Algorithm");
        }
	}

    private void ReadScoreBoard(){
        // string text = nameField.ReadAllText(@"./scores.json");
        string text = File.ReadAllText(Application.dataPath + "/Scripts/scores.json");
        listPeople people = JsonUtility.FromJson<listPeople>(text);
        Item Latest = people.itemList[people.itemList.Count-1];
        people.itemList.Sort((p1,p2) => p1.timeTaken.CompareTo(p2.timeTaken));
        int LatestCount = people.itemList.IndexOf(Latest) + 1;

        string latestString = "Latest Completion\nPlacement: " + LatestCount + "/"+ people.itemList.Count + " Name: " + Latest.name + " Time Taken: " + Latest.timeTaken;
        GameObject latestOutput = GameObject.Find("Latest");
        latestOutput.GetComponent<TextMeshProUGUI>().text = latestString;

        string display = "\tName\t\tTime Taken\n";
        int scoreboardLength = 0;
        if (people.itemList.Count > 5){
            scoreboardLength = 5;
        }
        else{
            scoreboardLength = people.itemList.Count;
        }
        for (int i = 0; i < scoreboardLength; i ++){
            display += i+1 + "\t" + people.itemList[i].name;
            if (people.itemList[i].name.Length < 10){
                display += "\t";
            }
            if (people.itemList[i].name.Length < 6){
                display += "\t";
            }
            display += people.itemList[i].timeTaken;
            if (i != 4) {
                display += "\n";
            }
        };
        GameObject.Find("ScoreBoard").GetComponent<TextMeshProUGUI>().text = display;
        print(display);
        // return null;
    }

    private void AddToJson(){
        string text = File.ReadAllText(Application.dataPath + "/Scripts/scores.json");
        listPeople people = JsonUtility.FromJson<listPeople>(text);

        var listPeople = new listPeople(){
            itemList = new List<Item>{}
        };
        foreach (Item item in people.itemList){
            listPeople.itemList.Add(item);
        }
        listPeople.itemList.Add(new Item{name = scores.name, timeTaken = scores.timeTaken});
        string json = JsonUtility.ToJson(listPeople, true);
        File.WriteAllText(Application.dataPath + "/Scripts/scores.json", json);
    }

    [System.Serializable]
    public class Item{
        public string name;
        public int timeTaken;
    }

    [System.Serializable]
    public class listPeople{
        public List<Item> itemList;
    }
}

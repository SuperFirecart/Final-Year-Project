using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    private GameObject playGameButton;
    private GameObject settingsButton;
    private GameObject exitButton;
    // Start is called before the first frame update
    void Start()
    {
        print("main menu");
        playGameButton = transform.GetChild(2).gameObject;
        settingsButton = transform.GetChild(3).gameObject;
        exitButton = transform.GetChild(4).gameObject;
        playGameButton.GetComponent<Button>().onClick.AddListener(PlayOnClick);
        settingsButton.GetComponent<Button>().onClick.AddListener(SettingsOnClick);
        exitButton.GetComponent<Button>().onClick.AddListener(ExitOnClick);
        print(playGameButton + " " + settingsButton + " " + exitButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayOnClick(){
        SceneManager.LoadScene("Tutorial");
	}
    void SettingsOnClick(){
        
		print("You have clicked the button!");
        SceneManager.LoadScene("Settings");
	}
    void ExitOnClick(){
		print("You have clicked the Exit Button!");
	}
}

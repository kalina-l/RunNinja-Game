using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour {

    public int numOfPlayer = 0;
    public bool[] playerToPlay;  //the ids of the player

    public GameObject player1Label;
    public GameObject player2Label;
    public GameObject player3Label;
    public GameObject player4Label;
    private GameObject[] playerLabels;

    public GameObject joinLabel;
    public GameObject startLabel;
    public GameObject timeLeftLabel;

    private bool startPhase;
    private bool inGame;
    private double timeLeftUntilStart = 3;

    //TO Delete after Start of Game
    public GameObject canvas;
    public GameObject eventsystem;
    public GameObject menueCamera;

    // Use this for initialization
    void Start () {
        startPhase = false;
        inGame = false;
        playerLabels = new GameObject[4];
        playerToPlay = new bool[4];
        playerLabels[0] = player1Label;
        playerLabels[1] = player2Label;
        playerLabels[2] = player3Label;
        playerLabels[3] = player4Label;

        for (int i = 0; i < playerLabels.Length; i++)
        {
            Debug.Log("Disable PlayerLabel "+i);
            playerLabels[i].SetActive(false);
            playerToPlay[i] = false;          
        }
        joinLabel.SetActive(false);
        timeLeftLabel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inGame)
        {
            if (!startPhase)
            {
                for (int i = 1; i <= 4; i++)
                {
                    string controlAccess = Controls.GetControlValue(Controls.Input.Jump, i);
                    if (Input.GetButtonDown(controlAccess))
                    {
                        Debug.Log(controlAccess + " pressed");
                        playerLabels[i - 1].SetActive(true);
                        playerToPlay[i - 1] = true;
                    }
                }

                for (int i = 1; i <= 4; i++)
                {
                    string controlAccess = Controls.GetControlValue(Controls.Input.Attack, i);
                    if (Input.GetButtonDown(controlAccess))
                    {
                        Debug.Log(controlAccess + " pressed");
                        playerLabels[i - 1].SetActive(false);
                        playerToPlay[i - 1] = false; 
                    }
                }


                int curentNumOfPlayers = numOfActivePlayer();
                if (curentNumOfPlayers == 0)
                {
                    joinLabel.SetActive(true);
                    startLabel.SetActive(false);
                }
                if (curentNumOfPlayers == 1)
                {
                    joinLabel.SetActive(false);
                    startLabel.SetActive(false);
                }
                if (curentNumOfPlayers >= 2)
                {
                    joinLabel.SetActive(false);
                    startLabel.SetActive(true);
                }

                if (Input.GetButtonDown("Start"))
                {
                    Debug.Log("Start Button pressed");
                    if (curentNumOfPlayers >= 2)
                    {
                        //game can start
                        startPhase = true;
                        timeLeftLabel.SetActive(true);
                        startLabel.SetActive(false);
                    }
                }
            }
            else
            {
                //inside the start phase
                timeLeftUntilStart -= Time.deltaTime;
                timeLeftLabel.GetComponent<Text>().text = String.Format("{0:0.00}", timeLeftUntilStart);

                if (timeLeftUntilStart < 0)
                {
                    //make scenechange
                    timeLeftLabel.GetComponent<Text>().text = "Start";
                    startGame(numOfActivePlayer());
                }
            }
        }
    }

    private void startGame(int nNumOfPlayers)
    {
        numOfPlayer = nNumOfPlayers;

        //remove elements from current Scene exept this one - make sure nothings runs in this script.
        inGame = true;
        Destroy(menueCamera);
        Destroy(canvas);
        Destroy(eventsystem);

        //load additive the new scene
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }

    private int numOfActivePlayer()
    {
        int num = 0;
        for (int i = 0; i < playerLabels.Length; i++)
        {
            if (playerLabels[i].active)
            {
                num++;
            }
        }
        return num;
    }
}

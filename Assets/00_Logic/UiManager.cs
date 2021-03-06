﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour {

	public static UiManager Instance;

    public int numOfPlayer = 0;
    public bool[] playerToPlay;  //the ids of the player

	public GameObject menuPanel;

    public GameObject player1Label;
    public GameObject player2Label;
    public GameObject player3Label;
    public GameObject player4Label;
	public GameObject legendPanel;
    private GameObject[] playerLabels;

    public GameObject joinLabel;
    public GameObject startLabel;
    public GameObject timeLeftLabel;

    private bool startPhase;
    private bool inGame;
    private double timeLeftUntilStart = 3;

	public GameObject gameplayPanel;

	public Material player1Mat;
	public Material player2Mat;
	public Material player3Mat;
	public Material player4Mat;
	public Image playerDiedImage;
	public Text playerDiedText;

	public Text stageText;
	private int stageCounter = 1;
	public int stages = 2;
	public Image stageImage;
	public Sprite[] stageSprites;

	private bool inputDelay;

    //TO Delete after Start of Game
    public GameObject menueCamera;

	void Awake() {
		Instance = this;
	}

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
    void Update()
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

				for(int i = 1; i <= 4; i++)
				{
					Debug.Log ("Stage change pressed.");
					string controlAccess = Controls.GetControlValue(Controls.Input.Horizontal, i);
					if(playerToPlay[i-1] && ! inputDelay)
					{
						stageCounter += Mathf.Clamp((int)(Input.GetAxis (controlAccess) * 2), -1, 1);

						if (stageCounter <= 0)
							stageCounter = stages;
						else if (stageCounter > stages)
							stageCounter = 1;

						stageText.text = "STAGE " + stageCounter;
						stageImage.sprite = stageSprites [stageCounter - 1];

						StartCoroutine (InputDelay ());
					}
				}


                int curentNumOfPlayers = numOfActivePlayer();
                if (curentNumOfPlayers == 0)
                {
                    joinLabel.SetActive(true);
                    startLabel.SetActive(false);
					legendPanel.SetActive (false);
                }
                if (curentNumOfPlayers == 1)
                {
                    joinLabel.SetActive(false);
                    startLabel.SetActive(false);
					legendPanel.SetActive (true);
                }
                if (curentNumOfPlayers >= 2)
                {
                    joinLabel.SetActive(false);
                    startLabel.SetActive(true);
					legendPanel.SetActive (true);
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
		menuPanel.SetActive (false);
		gameplayPanel.SetActive (true);

        //load additive the new scene
        SceneManager.LoadScene("TestMultiplePlayer", LoadSceneMode.Additive);


		SceneManager.LoadScene("LevelTester_" + stageCounter, LoadSceneMode.Additive);
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

	public void playerDied(int playerNumber){
		playerDiedText.text = "defeated!";
		playerDiedImage.enabled = true;
		switch (playerNumber) {
		case 1:
			playerDiedImage.material = player1Mat;
			break;
		case 2:
			playerDiedImage.material = player2Mat;
			break;
		case 3:
			playerDiedImage.material = player3Mat;
			break;
		case 4:
			playerDiedImage.material = player4Mat;
			break;

		}
		StartCoroutine (deactivePlayerDiedLabel());
	}

	private IEnumerator deactivePlayerDiedLabel(){
		yield return new WaitForSeconds(3);
		playerDiedText.text = "";
		playerDiedImage.enabled = false;
	}

	private IEnumerator InputDelay()
	{
		inputDelay = true;

		yield return new WaitForSeconds (0.2f);

		inputDelay = false;
	}
}

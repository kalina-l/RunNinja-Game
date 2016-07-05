﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class ParticipantManager : MonoBehaviour
{

    public ArrayList participants;
    public GameObject partipantPrefab;

    private bool activeGame = false;
    private bool preFreeze = true;
    private float preFrezzeTime = 3;

    private Camera cam;
    private FollowCamera followCam;
    private UiManager uiManager; // From Scene before

    public static ParticipantManager instance { get; private set; }

    public Material[] playerMaterials;

    void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start()
    {
        participants = new ArrayList();
        uiManager = GameObject.FindWithTag("myUIController").GetComponent<UiManager>();
        bool[] playerIDs = uiManager.playerToPlay;
        activeGame = true;
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerIDs[i])
            {
                //instantiate that one
                AddParticipant(i + 1);

            }
        }
        cam = GameObject.FindWithTag("myCamera").GetComponent<Camera>();
        followCam = cam.GetComponent("FollowCamera") as FollowCamera;
        SceneManager.LoadScene("LevelTester", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

        if (activeGame)
        {
            int numOfActivePlayers = this.getNumOIfActivePlayers();

            //check If somebody won the game
            if (numOfActivePlayers == 1)
            {
                //end of game
                Debug.Log("end of game");
                endOfGame();
            }

            else if (numOfActivePlayers != 0)
            {
                //remove players
                removeOuterParticipants();


                //Debug.Log("Find leading Player");
                Participant leader = getLeadingParticipant();
                if (leader == null)
                {
                    followCam.target = cam.gameObject;
                }
                else
                {
                    followCam.target = leader.character;
                }

            }
        }
    }

    private void endOfGame()
    {
        activeGame = false;
        SceneManager.LoadScene("WinnerScene", LoadSceneMode.Additive);
    }

    private float calcShadowBorder()
    {
        var aspectRatio = cam.aspect;
        var leftBounds = cam.transform.position.x - cam.orthographicSize * aspectRatio;
        var width = cam.orthographicSize * aspectRatio * 2;
        GameObject shadow = cam.transform.FindChild("Shadow").gameObject;        
        ParticleSystem particelSystem = shadow.GetComponent<ParticleSystem>();
        var shadowSize = particelSystem.shape.box.x;
        float y2 = leftBounds + width;
        float y1 = leftBounds;
        float x1 = -28f;
        float x2 = 0;
        float x = shadow.transform.localPosition.x;
        float y = ((y2 - y1) / (x2 - x1)) * (x - x1) + y1;
        return y;
    }

    private void removeOuterParticipants()
    {
        var leftbounds = calcShadowBorder();

        foreach (Participant p in participants)
        {
            if (p.isAlive)
            {
                var playerX = p.character.transform.position.x;
                if (playerX < leftbounds)
                {
                    //KILL
                    RemoveParticipant(p);
                }
            }
        }
    }

    public Participant getLeadingParticipant()
    {
        Participant leader = null;
        foreach (Participant p in participants)
        {
            if (p.isAlive)
            {
                if (leader == null)
                {
                    leader = p;
                }
                else if (p.character.transform.position.x > leader.character.transform.position.x)
                {
                    leader = p;
                }
            }
        }
        return leader;
    }

    void FixedUpdate()
    {

    }

    void AddParticipant(int id)
    {
        GameObject clone;
        clone = Instantiate(partipantPrefab,
                            partipantPrefab.transform.position,
                            partipantPrefab.transform.rotation) as GameObject;



        Participant part = clone.GetComponent<Participant>();

        part.setId(id);
        participants.Add(part);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene

        p.isAlive = false;
        GameObject character = p.character;
        Destroy(character);
		uiManager.playerDied (p.id);
    }

    int getNumOIfActivePlayers()
    {
        int num = 0;
        foreach (Participant p in participants)
        {
            if (p.isAlive)
            {
                num++;
            }
        }
        return num;
    }
}

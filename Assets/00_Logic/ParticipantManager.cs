using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class ParticipantManager : MonoBehaviour {

    public ArrayList participants;
    public GameObject partipantPrefab;
    private bool activeGame = false;

    private Camera cam;
    private FollowCamera followCam;
    private UiManager uiManager; // From Scene before

    public static ParticipantManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    

    // Use this for initialization
    void Start () {
        participants = new ArrayList();
        uiManager = GameObject.FindWithTag("myUIController").GetComponent<UiManager>();
        bool[] playerIDs = uiManager.playerToPlay;
        activeGame = true;
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (playerIDs[i])
            {
                //instantiate that one
                AddParticipant(i+1);

            }
        }       
        cam = GameObject.FindWithTag("myCamera").GetComponent <Camera> ();
        followCam = cam.GetComponent("FollowCamera") as FollowCamera;
    }
	
	// Update is called once per frame
	void Update () {

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
                    followCam.target = leader.transform.Find("Character").gameObject;
                }

            } 
        }
    }

    private void endOfGame()
    {
        activeGame = false;
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
    }

    private void removeOuterParticipants()
    {
        var pos = cam.transform.position;
        //Debug.Log("Pos Cam " + pos);

        var aspectRatio = cam.aspect;
        //Debug.Log("aspectRatio " + aspectRatio);

        var leftBounds = cam.transform.position.x - cam.orthographicSize*aspectRatio;
        //Debug.Log("leftBounds " + leftBounds);

        foreach (Participant p in participants)
        {
            if (p.isAlive)
            {
                var playerX = p.transform.Find("Character").position.x;
                //Debug.Log("playerX "+ playerX);
                if (playerX < leftBounds)
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
                //Debug.Log(p.transform.Find("Character").position.x);
                else if (p.transform.Find("Character").position.x > leader.transform.Find("Character").position.x)
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



        Participant part = clone.GetComponent("Participant") as Participant;

        part.setId(id);
        participants.Add(part);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene

        p.isAlive = false;
        GameObject character = p.transform.Find("Character").gameObject;
        Destroy(character);
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

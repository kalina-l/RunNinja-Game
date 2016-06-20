using UnityEngine;
using System.Collections;
using System;

public class ParticipantManager : MonoBehaviour {

    public ArrayList participants;
    public GameObject partipantPrefab;

    private Camera cam;
    private FollowCamera followCam;
    private UiManager uiManager; // From Scene before
    public int numOfPlayers;

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

        if (participants.Count != 0)
        {
            //Debug.Log("Find leading Player");
            Participant leader = getLeadingParticipant();
            followCam.target = leader.transform.Find("Character").gameObject;
        }
    }

    private Participant getLeadingParticipant()
    {
        Participant leader = (Participant)participants.ToArray()[0];
        foreach (Participant p in participants)
        {
            Debug.Log(p.transform.Find("Character").position.x);
            if (p.transform.Find("Character").position.x > leader.transform.Find("Character").position.x)
            {
                leader = p;
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

        numOfPlayers++; 

        Participant part = clone.GetComponent("Participant") as Participant;

        part.setId(id);
        participants.Add(part);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene
        //participants.Remove(p);
    }
}

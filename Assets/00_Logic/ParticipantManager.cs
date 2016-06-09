using UnityEngine;
using System.Collections;
using System;

public class ParticipantManager : MonoBehaviour {

    public ArrayList participants;
    public GameObject partipantPrefab;
    CameraController camController;

    private int numOfPlayers;

    public static ParticipantManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    

    // Use this for initialization
    void Start () {
        this.numOfPlayers = 0;
        participants = new ArrayList();
        camController = GetComponent("CameraController") as CameraController;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //Debug.Log("Add new Particpant");
            AddParticipant();
        }

        if (participants.Count != 0)
        {
            //Debug.Log("Find leading Player");
            Participant leader = getLeadingParticipant();
            camController.leadingPlayer = leader.gameObject;
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

    void AddParticipant()
    {
        GameObject clone;
        clone = Instantiate(partipantPrefab,
                            partipantPrefab.transform.position,
                            partipantPrefab.transform.rotation) as GameObject;

        //Debug.Log("Type of clone "+ clone.GetType());

        numOfPlayers++; 

        Participant part = clone.GetComponent("Participant") as Participant;
        //Debug.Log("Type of part " + part.GetType());

        part.setId(numOfPlayers);
        participants.Add(part);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene
        //participants.Remove(p);
    }
}

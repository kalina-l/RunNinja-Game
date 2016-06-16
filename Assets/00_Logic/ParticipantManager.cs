using UnityEngine;
using System.Collections;
using System;

public class ParticipantManager : MonoBehaviour {

    public ArrayList participants;
    public GameObject partipantPrefab;

    private Camera cam;
    private FollowCamera followCam;
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
        cam = GameObject.FindWithTag("myCamera").GetComponent <Camera> ();
        //Debug.Log("cam " + cam.GetType());
        followCam = cam.GetComponent("FollowCamera") as FollowCamera;
        //Debug.Log("followCam " + followCam.GetType());

    }
	
	// Update is called once per frame
	void Update () {

        if (participants.Count != 0)
        {
            //Debug.Log("Find leading Player");
            Participant leader = getLeadingParticipant();
            followCam.target = leader.transform.Find("Character").gameObject;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //Debug.Log("Add new Particpant");
            AddParticipant();
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

        numOfPlayers++; 

        Participant part = clone.GetComponent("Participant") as Participant;

        part.setId(numOfPlayers);
        participants.Add(part);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene
        //participants.Remove(p);
    }
}

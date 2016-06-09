using UnityEngine;
using System.Collections;

public class ParticipantManager : MonoBehaviour {

    //public ArrayList participants;
    public GameObject partipantPrefab;

    private int numOfPlayers;

    public static ParticipantManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    

    // Use this for initialization
    void Start () {
        this.numOfPlayers = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Add new Particpant");
            AddParticipant();
        }
	
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
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene
        //participants.Remove(p);
    }
}

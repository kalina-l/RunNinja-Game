using UnityEngine;
using System.Collections;

public class ParticipantManager : MonoBehaviour {

    private ArrayList participants;

	// Use this for initialization
	void Start () {
        participants = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void AddParticipant()
    {
        //Participant p = new Participant();
    }

    void RemoveParticipant(Participant p)
    {
        participants.Remove(p);
    }
}

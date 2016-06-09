using UnityEngine;
using System.Collections;

public class ParticipantManager : MonoBehaviour {

    //public ArrayList participants;
    public Transform partipantPrefab;

    // Use this for initialization
    void Start () {
        //participants = new ArrayList();
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
        //Add Prefab to scene
        var go = Instantiate(partipantPrefab, transform.position, transform.rotation);
    }

    void RemoveParticipant(Participant p)
    {
        //TODO Remove from scene
        //participants.Remove(p);
    }
}

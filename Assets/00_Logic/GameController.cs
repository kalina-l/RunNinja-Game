using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    /*
    Für allgemeines Zeug wie Screens, Menüs, Gamestates usw.
    */

    private ParticipantManager participantManager;

    public static GameController instance { get; private set; }

    void Awake()
    {
        instance = this;
    }


	// Use this for initialization
	void Start () {
        participantManager = new ParticipantManager();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

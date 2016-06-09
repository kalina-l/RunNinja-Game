using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    /*
    Für allgemeines Zeug wie Screens, Menüs, Gamestates usw.
    */

    public static GameController instance { get; private set; }

    void Awake()
    {
        instance = this;
    }


	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

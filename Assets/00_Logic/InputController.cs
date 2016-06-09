using UnityEngine;
using System.Collections;

public class InputController: MonoBehaviour {

    // Eingabe Tastatur, controller 
    public int idOfPlayer;

    public static InputController instance { get; private set; }

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

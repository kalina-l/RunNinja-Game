using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject leadingPlayer;
    public Camera camera;

    // Für die Kamerabwegung / steuerung / Modi - Spielerverfolgerung

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate(){
        if(leadingPlayer != null)
        {
            camera.transform.position = leadingPlayer.transform.Find("Character").position;
        }
        
	}
}

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
            Vector3 pos = leadingPlayer.transform.Find("Character").position;
            Vector3 newPos = camera.transform.position;
            newPos.x = pos.x;
            camera.transform.position = newPos;
        }
        
	}
}

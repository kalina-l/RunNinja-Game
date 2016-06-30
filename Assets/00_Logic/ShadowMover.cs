using UnityEngine;
using System.Collections;

public class ShadowMover : MonoBehaviour {

    public double startTimeOfMovement;
    public float velocity;
    public GameObject shadow;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    startTimeOfMovement -= Time.deltaTime;
        if (startTimeOfMovement <= 0)
        {
            //move shadow
            Vector3 pos = shadow.transform.localPosition;
            Vector3 newPos = new Vector3(pos.x + velocity, pos.y, pos.z);
            shadow.transform.localPosition = newPos;
        }
    }
}

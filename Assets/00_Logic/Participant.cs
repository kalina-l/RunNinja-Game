using UnityEngine;
using System.Collections;

public class Participant : MonoBehaviour {

    public int id;
    public string alias;
    public bool isAlive;

    // Use this for initialization
    void Start () {
        isAlive = true;
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.position = transform.Find("Character").position;

    }
    public void setId(int value)
    {
        //Debug.Log(id);
        id = value;
        GameObject go = transform.Find("Character").gameObject;
        //Debug.Log("go "+ go.GetType());
        PlayerControl pc = go.GetComponent("PlayerControl") as PlayerControl;
        //Debug.Log("pc " + pc.GetType());
        pc.control_id = id;
    }
}

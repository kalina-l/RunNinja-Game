using UnityEngine;
using System.Collections;

public class Participant : MonoBehaviour {

    public int id;
    public string alias;
    public bool isAlive;
    public GameObject character;

    public SpriteRenderer renderer;

    // Use this for initialization
    void Start () {
        isAlive = true;
    }
	
	// Update is called once per frame
	void Update () {

    }
    public void setId(int value)
    {
        //Debug.Log(id);
        id = value;
        PlayerControl pc = character.GetComponent("PlayerControl") as PlayerControl;
        pc.control_id = id;
        renderer.material = ParticipantManager.instance.playerMaterials[value-1];
    }
}

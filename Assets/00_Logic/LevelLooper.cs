using UnityEngine;
using System.Collections;

public class LevelLooper : MonoBehaviour {

    public GameObject levelPrefab;
    public GameObject lastInsertedLevel = null;
    public float widthOfLevel;
    public float offsetOfLevel;
    public ParticipantManager participantManager;

	// Use this for initialization
	void Start () {
        this.participantManager = GameObject.Find("ParticipantManager").gameObject.GetComponent<ParticipantManager>();

        lastInsertedLevel = Instantiate(levelPrefab,
                            levelPrefab.transform.position,
                            levelPrefab.transform.rotation) as GameObject;

        BoxCollider2D levelBox = levelPrefab.GetComponent<BoxCollider2D>();
        this.widthOfLevel = levelBox.size.x;
        this.offsetOfLevel = levelBox.offset.x;

    }
	
	// Update is called once per frame
	void Update () {
        Participant leader = participantManager.getLeadingParticipant();
        if (leader != null)
        {
            float leaderX = leader.transform.Find("Character").position.x;
            float levelX = lastInsertedLevel.transform.position.x + this.offsetOfLevel - this.widthOfLevel/2;
            //Debug.Log("leaderX "+ leaderX);
            //Debug.Log("levelX " + levelX);
            if (leaderX >= levelX)
            {
                Vector3 lastPos = lastInsertedLevel.transform.position;
                Vector3 newPos = new Vector3(lastPos.x + widthOfLevel, 0, 0);

                //create a new level
                lastInsertedLevel = Instantiate(levelPrefab,
                                    newPos,
                                    levelPrefab.transform.rotation) as GameObject;
            }
        }
    }
}

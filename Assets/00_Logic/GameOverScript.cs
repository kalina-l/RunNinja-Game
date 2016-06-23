using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

    public int idOfWinner;
    public GameObject winnerLabel;

	// Use this for initialization
	void Start () {


        GameObject partManager = GameObject.Find("ParticipantManager").gameObject;
        ParticipantManager participantManager = partManager.GetComponent<ParticipantManager>();
        idOfWinner = participantManager.getLeadingParticipant().id;

        winnerLabel.GetComponent<Text>().text = "Player "+idOfWinner;
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 1; i <= 4; i++)
        {
            string controlAccess = Controls.GetControlValue(Controls.Input.Jump, i);
            if (Input.GetButtonDown(controlAccess))
            {
                Debug.Log(controlAccess + " pressed");
                //Restart Game
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

    public int idOfWinner;
    public GameObject winnerLabel;
    public GameObject pressAToStart;

    public float freezeTimeAfterGame = 2;
    private bool isFrozen;

	// Use this for initialization
	void Start () {


        GameObject partManager = GameObject.Find("ParticipantManager").gameObject;
        ParticipantManager participantManager = partManager.GetComponent<ParticipantManager>();
        idOfWinner = participantManager.getLeadingParticipant().id;
        pressAToStart.SetActive(false);

        winnerLabel.GetComponent<Text>().text = "Player "+idOfWinner;
        isFrozen = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isFrozen)
        {
            for (int i = 1; i <= 4; i++)
            {
                string controlAccess = Controls.GetControlValue(Controls.Input.Jump, i);
                if (Input.GetButtonDown(controlAccess))
                {
                    Debug.Log(controlAccess + " pressed");
                    //Restart Game
                    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                }
            } 
        } else {
            //inside the start phase
            freezeTimeAfterGame -= Time.deltaTime;

            if (freezeTimeAfterGame < 0)
            {
                isFrozen = false;
                pressAToStart.SetActive(true);
            }
        }
    }
}

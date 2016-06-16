using UnityEngine;
using System.Collections;

public class UiManager : MonoBehaviour {

    public GameObject player1Label;
    public GameObject player2Label;
    public GameObject player3Label;
    public GameObject player4Label;
    private GameObject[] playerLabels;

    public GameObject joinLabel;
    public GameObject startLabel;

    // Use this for initialization
    void Start () {
        playerLabels = new GameObject[4];
        playerLabels[0] = player1Label;
        playerLabels[1] = player2Label;
        playerLabels[2] = player3Label;
        playerLabels[3] = player4Label;

        for (int i = 0; i < playerLabels.Length; i++)
        {
            Debug.Log("Disable PlayerLabel "+i);
            playerLabels[i].SetActive(false);            
        }
        joinLabel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 1; i <= 4; i++)
        {
            string controlAccess = Controls.GetControlValue(Controls.Input.Jump, i);
            if (Input.GetButtonDown(controlAccess))
            {
                Debug.Log(controlAccess + " pressed");
                playerLabels[i-1].SetActive(true);
            }
        }

        for (int i = 1; i <= 4; i++)
        {
            string controlAccess = Controls.GetControlValue(Controls.Input.Attack, i);
            if (Input.GetButtonDown(controlAccess))
            {
                Debug.Log(controlAccess + " pressed");
                playerLabels[i - 1].SetActive(false);
            }
        }


        int curentNumOfPlayers = numOfActivePlayer();
        if (curentNumOfPlayers == 0)
        {
            joinLabel.SetActive(true);
            startLabel.SetActive(false);
        }
        if (curentNumOfPlayers == 1)
        {
            joinLabel.SetActive(false);
            startLabel.SetActive(false);
        }
        if (curentNumOfPlayers >= 2)
        {
            joinLabel.SetActive(false);
            startLabel.SetActive(true);
        }
    }

    private int numOfActivePlayer()
    {
        int num = 0;
        for (int i = 0; i < playerLabels.Length; i++)
        {
            if (playerLabels[i].active)
            {
                num++;
            }
        }
        return num;
    }
}

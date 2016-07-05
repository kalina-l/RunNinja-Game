using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpManager : MonoBehaviour 
{
    public GameObject[] PowerUps;



    private IPowerUp[] powerUpInstance;

    void Start()
    {
        powerUpInstance = new IPowerUp[PowerUps.Length];

        for (int i = 0; i < PowerUps.Length; i++)
        {
            GameObject pu = GameObject.Instantiate(PowerUps[i]) as GameObject;
            pu.transform.parent = transform;
            powerUpInstance[i] = pu.GetComponent<IPowerUp>();
        }
    }


    public void AddPowerUp(PlayerControl player)
    {
		Participant p = player.Participant;

		int racePosition = 0;

		if(ParticipantManager.instance != null)
			racePosition = ParticipantManager.instance.getPlayerRacePosition (p.id);

		List<IPowerUp> weightedList = new List<IPowerUp> ();

		for (int i = 0; i < powerUpInstance.Length; i++) {

			int weight = powerUpInstance [i].GetWeight (racePosition);

			for(int j=0; j<weight; j++)
			{
				weightedList.Add (powerUpInstance [i]);
			}
		}

		player.AddPowerUp(weightedList[(int)(Random.value * weightedList.Count)]);
    }
}

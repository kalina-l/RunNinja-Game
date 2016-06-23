using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour 
{
    public GameObject[] PowerUps;



    private IPowerUp[] powerUpInstance;

    void Start()
    {
        powerUpInstance = new IPowerUp[PowerUps.Length];

        for (int i = 0; i < PowerUps.Length; i++)
        {
            powerUpInstance[i] = PowerUps[i].GetComponent<IPowerUp>();
        }
    }


    public void AddPowerUp(PlayerControl player)
    {
        player.AddPowerUp(powerUpInstance[(int)(Random.value * PowerUps.Length)]);
    }
}

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
            GameObject pu = GameObject.Instantiate(PowerUps[i]) as GameObject;
            pu.transform.parent = transform;
            powerUpInstance[i] = pu.GetComponent<IPowerUp>();
        }
    }


    public void AddPowerUp(PlayerControl player)
    {
        player.AddPowerUp(powerUpInstance[(int)(Random.value * PowerUps.Length)]);
    }
}

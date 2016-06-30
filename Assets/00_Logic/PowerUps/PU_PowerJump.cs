using UnityEngine;
using System.Collections;

public class PU_PowerJump : MonoBehaviour, IPowerUp {

    public Vector2 BoostForce;

    private PlayerControl player;

    public void Setup(PlayerControl player)
    {
        Debug.Log("Picked up POWERJUMP!");
        //something happens when I pick this up
        this.player = player;
    }

    public void Activate()
    {
        Debug.Log("Speed Booooooost!!!!");

        //something happens when I activate this
        player.BoostPlayer(BoostForce);
        player.RemovePowerUp();
    }
}

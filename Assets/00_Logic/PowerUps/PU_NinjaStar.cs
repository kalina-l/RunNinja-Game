using UnityEngine;
using System.Collections;

public class PU_NinjaStar : MonoBehaviour, IPowerUp {

    public GameObject NinjStar;
    private PlayerControl player;

    private int starCount;

    public void Setup(PlayerControl player)
    {
        //something happens when I pick this up
        this.player = player;
        starCount = 3;
    }

    public void Activate()
    {
        Debug.Log("Speed Booooooost!!!!");

        //something happens when I activate this
        GameObject star = GameObject.Instantiate(NinjStar, player.ProjectilePoint.position, player.ProjectilePoint.rotation) as GameObject;
        star.GetComponent<NinjaStarController>().Init(player.facingRight);

        starCount--;

        if (starCount == 0)
        {
            player.RemovePowerUp();
        }
    }
}

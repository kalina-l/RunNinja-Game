using UnityEngine;
using System.Collections;

public class PU_NinjaStar : MonoBehaviour, IPowerUp {

    public GameObject NinjStar;
    private PlayerControl player;

    public int starCount = 3;

    public void Setup(PlayerControl player)
    {
        //something happens when I pick this up
        this.player = player;
    }

    public void Activate()
    {
        //something happens when I activate this
        GameObject star = GameObject.Instantiate(NinjStar, player.ProjectilePoint.position, player.ProjectilePoint.rotation) as GameObject;
        star.GetComponent<NinjaStarController>().Init(player.facingRight);

        //starCount--;

        if (starCount == 0)
        {
            player.RemovePowerUp();
        }
    }
}

using UnityEngine;
using System.Collections;

public class PU_NinjaStar : MonoBehaviour, IPowerUp {

    public GameObject NinjStar;
    private PlayerControl player;

    public int starCount = 3;
    private int currentStars;
    public Sprite icon;

    public void Setup(PlayerControl player)
    {
        //something happens when I pick this up
        this.player = player;
        currentStars = starCount;
    }

    public void Activate()
    {
        //something happens when I activate this
        GameObject star = GameObject.Instantiate(NinjStar, player.ProjectilePoint.position, player.ProjectilePoint.rotation) as GameObject;
        star.GetComponent<NinjaStarController>().Init(player.facingRight, player);

        currentStars--;

        if (currentStars <= 0)
        {
            player.RemovePowerUp();
        }
    }

    public Sprite GetIcon()
    {
        return icon;
    }
}

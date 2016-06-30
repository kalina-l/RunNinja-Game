using UnityEngine;
using System.Collections;

public class PU_ShadowRun : MonoBehaviour, IPowerUp {

    public float duration = 5;
    public GameObject ShadowFX;
    public Sprite icon;
    public int weight;


    private PlayerControl player;

    public void Setup(PlayerControl player)
    {
        Debug.Log("Picked up SHADOWRUN!");

        this.player = player;
    }

    public void Activate()
    {
        //something happens when I activate this
        player.ActivateShadowForm(ShadowFX, duration);
        player.RemovePowerUp();
    }

    public Sprite GetIcon()
    {
        return icon;
    }
    public int GetWeight()
    {
        return weight;
    }

}

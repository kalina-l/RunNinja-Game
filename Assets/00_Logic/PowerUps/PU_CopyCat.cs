using UnityEngine;
using System.Collections;

public class PU_CopyCat : MonoBehaviour, IPowerUp
{

    public GameObject Bunshin;
    private PlayerControl player;

    public Sprite icon;
    public int[] weight;

    public int count = 3;
    private int currentCopies;

    public void Setup(PlayerControl player)
    {
        //something happens when I pick this up
        this.player = player;
        currentCopies = count;
    }

    public void Activate()
    {
        //something happens when I activate this
        GameObject bunshin = GameObject.Instantiate(Bunshin, player.transform.position, player.transform.rotation) as GameObject;
        bunshin.GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite;

        currentCopies--;

        if (currentCopies <= 0)
        {
            player.RemovePowerUp();
        }
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetWeight(int place)
    {
        return weight[place];
    }
}

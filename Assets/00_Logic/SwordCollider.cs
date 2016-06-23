using UnityEngine;
using System.Collections;

public class SwordCollider : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            other.GetComponent<Obstacle>().Destroy();
        }
    }
}

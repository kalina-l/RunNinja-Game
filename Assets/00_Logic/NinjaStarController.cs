
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NinjaStarController : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private bool destroyed;
    private float timer;

    public bool swap;

    private PlayerControl player;

	// Use this for initialization
	public void Init (bool facingRight, PlayerControl player) {
        rigidBody = GetComponent<Rigidbody2D>();

        Vector2 force = Vector2.right * speed;

        if (!facingRight)
            force = Vector2.right * speed * -1;

        rigidBody.AddForce(force, ForceMode2D.Impulse);
        animator = GetComponent<Animator>();

        this.player = player;
    }

    // Update is called once per frame
    void Update() {

        if (destroyed)
        {
            timer += Time.deltaTime;

            if (timer > 0.25f)
            {
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer > 4)
            {
                Destroy();
            }
        }
    }

    private void Destroy()
    {
        destroyed = true;
        timer = 0;

        animator.SetTrigger("Destroy");

        rigidBody.velocity = Vector3.zero;

        GetComponent<Collider2D>().enabled = false;
    }

    void OnTriggerEnter2D (Collider2D other) {


        if (other.tag == "Player")
        {
            if (swap)
            {
                Vector3 playerPosition = player.transform.position;
                player.transform.position = other.transform.position;
                other.transform.position = playerPosition;
            }
            else
            {
                other.GetComponent<PlayerControl>().StunPlayer();
            }
        }

        if (other.tag == "Obstacle")
        {
            Vector3 playerPosition = player.transform.position;
            player.transform.position = other.transform.position;
            other.transform.position = playerPosition;

            other.GetComponent<Obstacle>().Destroy();
        }

        Destroy();
    }
}



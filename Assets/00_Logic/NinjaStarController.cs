
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NinjaStarController : MonoBehaviour {
    public float speed;
    private Rigidbody2D rigidBody;
    private Animator animator;

    private bool destroyed;
    private float timer;

	// Use this for initialization
	public void Init (bool facingRight) {
        rigidBody = GetComponent<Rigidbody2D>();

        Vector2 force = Vector2.right * speed;

        if (!facingRight)
            force = Vector2.right * speed * -1;

        rigidBody.AddForce(force, ForceMode2D.Impulse);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        //rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        if (destroyed)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D (Collider2D other) {

        if (other.tag == "Player")
        {
             other.GetComponent<PlayerControl>().StunPlayer();
        }

        if (other.tag == "Obstacle")
        {
            other.GetComponent<Obstacle>().Destroy();
        }

        destroyed = true;

        animator.SetTrigger("Destroy");

        GetComponent<Collider2D>().enabled = false;
    }
}



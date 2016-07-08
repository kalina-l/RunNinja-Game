using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
    public bool IsLight;
    public float DestroyDelay = 1;

    private bool destroyed = false;

    private Collider2D collider;
    private Animator animator;
    private float timer;

    public float delay;
    private float delayTimer;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        if (delay > 0)
            collider.enabled = false;
    }

    void Update()
    {
        if (destroyed)
        {
            timer += Time.deltaTime;
            if (timer > DestroyDelay)
            {
                GameObject.Destroy(gameObject);
            }
        }
        else
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                collider.enabled = true;
            }
        }
    }

    public void Destroy()
    {
        if(animator != null)
            animator.SetTrigger("Destroy");
        collider.enabled = false;
        destroyed = true;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
			if(!other.gameObject.GetComponent<PlayerControl>().getRolling())
            other.gameObject.GetComponent<PlayerControl>().StunPlayer(0.35f);

            if (IsLight)
            {
                Destroy();
            }
        }

    }
}

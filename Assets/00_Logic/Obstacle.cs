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

    void Start()
    {
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
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
    }

    public void Destroy()
    {
        animator.SetTrigger("Destroy");
        collider.enabled = false;
        destroyed = true;
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControl>().StunPlayer();

            if (IsLight)
            {
                Destroy();
            }
        }

    }
}

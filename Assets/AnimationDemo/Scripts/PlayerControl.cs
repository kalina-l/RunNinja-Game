using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	private Vector3 collidersHalfWidth;

    public int control_id = 1;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float saveJumpHigh;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool doubleJumpUsed;
	private float highestJumpXValue = int.MinValue;
	private bool stunned;

	private Animator anim;					// Reference to the player's animator component.
	private Rigidbody2D rigidbody;

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
		Vector2 collidersSize = GetComponent<BoxCollider2D> ().size;
		rigidbody = GetComponent<Rigidbody2D> ();
		collidersHalfWidth = new Vector3 (collidersSize.x / 2, 0, 0);
	}


	void Update()
	{
		checkIfGrounded ();

        // If the jump button is pressed and the player is grounded then the player should jump.
        string controlAccess = Controls.GetControlValue(Controls.Input.Jump, this.control_id);
        //Debug.Log(controlAccess);
		if (Input.GetButtonDown (controlAccess) && !doubleJumpUsed) {
			//Debug.Log ("jump!");
			jump = true;
			if (!grounded) {	// if the player jumps but is already in the air, make double jump
				doubleJumpUsed = true;
			}
		}
	}


	void FixedUpdate ()
	{
        // Cache the horizontal input.
        string controlAccess = Controls.GetControlValue(Controls.Input.Horizontal, this.control_id);
        float h = Input.GetAxis(controlAccess);
		if (h < 0.1f && h > -0.1f && Mathf.Approximately(rigidbody.velocity.y, 0f)) {
			rigidbody.velocity = Vector2.zero;
			rigidbody.angularVelocity = 0f;
		}

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if (h * rigidbody.velocity.x < maxSpeed) {
			// ... add a force to the player.
			if(!stunned) AddForce (Vector2.right * h * moveForce, ForceMode.Impulse);
		}

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			flipPlayersDirection();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			flipPlayersDirection();

		anim.SetBool ("Fall", !grounded);
		anim.SetBool ("Jump", !grounded && rigidbody.velocity.y > 0);

		// If the player should jump...
		if(jump) {
			// Add a vertical force to the player.
			if(!stunned) AddForce(new Vector2(0f, jumpForce), ForceMode.Impulse);
			jump = false;
		}
	}

	private void checkIfGrounded(){
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.4
		if (Physics2D.Linecast (transform.position - collidersHalfWidth, groundCheck.position - collidersHalfWidth, 1 << LayerMask.NameToLayer ("Ground")) ||
			Physics2D.Linecast (transform.position + collidersHalfWidth, groundCheck.position + collidersHalfWidth, 1 << LayerMask.NameToLayer ("Ground"))) {
			grounded = true;
			doubleJumpUsed = false;
			if (Mathf.Abs (transform.position.y - highestJumpXValue) > saveJumpHigh) {
				rigidbody.velocity = Vector2.zero;
				rigidbody.angularVelocity = 0f;
				Debug.Log ("Stunned!");
				StartCoroutine (stunPlayer (15)); // stun for number of frames
				anim.SetTrigger ("Landing");
			}
			highestJumpXValue = transform.position.y;
		} else {
			grounded = false;
			if (transform.position.y > highestJumpXValue)
				highestJumpXValue = transform.position.y;
		}
	}

	void flipPlayersDirection ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private IEnumerator stunPlayer(int frameCount) {
		stunned = true;
		while (frameCount > 0)
		{
			frameCount--;
			yield return null;
		}
		stunned = false;
	}

	public void AddForce (Vector2 force, ForceMode mode ) {
		Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D> ();
		switch ( mode ){
		case ForceMode.Force:
			rigidbody2D.AddForce( force );
			break;
		case ForceMode.Impulse:
			rigidbody2D.AddForce( force / Time.fixedDeltaTime );
			break;
		case ForceMode.Acceleration:
			rigidbody2D.AddForce( force * rigidbody2D.mass );
			break;
		case ForceMode.VelocityChange:
			rigidbody2D.AddForce( force * rigidbody2D.mass / Time.fixedDeltaTime );
			break;
		}
	}
}

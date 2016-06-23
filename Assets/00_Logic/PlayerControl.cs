using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	private bool jump = false;				// Condition for whether the player should jump.
	private bool rolling = false;
	private bool attacking = false;
	private Vector3 collidersHalfWidth;

    public int control_id = 1;

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float saveJumpHeight;
	public bool blockJumpMovement;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool doubleJumpUsed;
	public bool falling;
	private float highestJumpXValue = int.MinValue;
	private bool stunned;

	private Animator anim;					// Reference to the player's animator component.
	private Rigidbody2D rigidbody;

    private IPowerUp currentPowerUp;

    public Transform ProjectilePoint;

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
		Debug.DrawRay(groundCheck.position, collidersHalfWidth + new Vector3 (2, 0, 0), Color.red, 1.0f, true);
		Debug.DrawRay(groundCheck.position, -collidersHalfWidth - new Vector3 (2, 0, 0), Color.red, 1.0f, true);

		checkIfGrounded ();
		checkFallsHeight ();

        // If the jump button is pressed and the player is grounded then the player should jump.
        string controlAccess = Controls.GetControlValue(Controls.Input.Jump, this.control_id);
		if (Input.GetButtonDown (controlAccess) && !doubleJumpUsed) {
			jump = true;
			if (!grounded) {	// if the player jumps but is already in the air, make double jump
				doubleJumpUsed = true;
			}
		}
		controlAccess = Controls.GetControlValue(Controls.Input.Roll, this.control_id);
		if (Input.GetButtonDown (controlAccess) && !stunned && falling) {
			anim.SetTrigger ("Roll");
		}
		controlAccess = Controls.GetControlValue(Controls.Input.Attack, this.control_id);
		if (Input.GetButtonDown (controlAccess)) {
			anim.SetTrigger ("Attack");
			attacking = true;
		}
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Roll"))
			rolling = true;
		else
			rolling = false;
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack"))
			attacking = true;
		else
			attacking = false;
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Fall"))
			blockJumpMovement = false;

        if (Input.GetButtonDown (Controls.GetControlValue(Controls.Input.Action, this.control_id)))
        {
            if (currentPowerUp != null)
            {
                currentPowerUp.Activate();
            }
        }
	}


	void FixedUpdate ()
	{
        // Cache the horizontal input.
        string controlAccess = Controls.GetControlValue(Controls.Input.Horizontal, this.control_id);
        float h = Input.GetAxis(controlAccess);
		// If the player doesn't run, jump or attack, set his velocity to 0 (to prevent sliding)
		if (h < 0.2f && h > -0.2f && Mathf.Approximately (rigidbody.velocity.y, 0f)) {
			if (!attacking) {
				rigidbody.velocity = Vector2.zero;
				rigidbody.angularVelocity = 0f;
			} else {
				if (facingRight)
					AddForce (Vector2.right * 2, ForceMode.Impulse);
				else
					AddForce (-Vector2.right * 2, ForceMode.Impulse);
			}
		}
		if (blockJumpMovement)
			h = 0;

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

		// If the player should jump...
		if(jump) {
			if (closeToWall() && !grounded) {
				wallJump ();
			} else {
				if (!stunned)
					AddForce (new Vector2 (0f, jumpForce), ForceMode.Impulse);
			}
			jump = false;
		}

		anim.SetBool ("Fall", !grounded);
		anim.SetBool ("Jump", !grounded && rigidbody.velocity.y > 0);

	}

	private void checkIfGrounded(){
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.4
		if (Physics2D.Linecast (transform.position - collidersHalfWidth, groundCheck.position - collidersHalfWidth, 1 << LayerMask.NameToLayer ("Ground")) ||
			Physics2D.Linecast (transform.position + collidersHalfWidth, groundCheck.position + collidersHalfWidth, 1 << LayerMask.NameToLayer ("Ground"))) {
			grounded = true;
			doubleJumpUsed = false;
			falling = false;
		} else {
			grounded = false;
		}
	}

	private void checkFallsHeight(){
		// stun if the player falls from a certain height
		if (grounded) {
			if (Mathf.Abs (transform.position.y - highestJumpXValue) > saveJumpHeight  && !rolling) {
				rigidbody.velocity = Vector2.zero;
				Debug.Log ("stun");
				StartCoroutine (stunPlayer (15)); // stun for number of frames
				anim.SetTrigger ("Landing");
			}
			highestJumpXValue = transform.position.y;
		}
		// otherwise calculate the height of the fall
		else {
			if (transform.position.y > highestJumpXValue) {
				highestJumpXValue = transform.position.y;
				falling = false;
			} else
				falling = true;
		}
	}

	void flipPlayersDirection ()
	{
		if (stunned || blockJumpMovement)
			return;
		
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private bool closeToWall(){
		if (Physics2D.Linecast (groundCheck.position, groundCheck.position + collidersHalfWidth + new Vector3 (2, 0, 0), 1 << LayerMask.NameToLayer ("Ground")) ||
			Physics2D.Linecast (groundCheck.position, groundCheck.position -collidersHalfWidth - new Vector3 (2, 0, 0), 1 << LayerMask.NameToLayer ("Ground"))) {
			return true;
		} else {
			return false;
		}
	}

	private void wallJump(){
		Debug.Log ("Wall Jump!");
		doubleJumpUsed = false;
		flipPlayersDirection ();
		//rigidbody.velocity = new Vector2(-rigidbody.velocity.x, rigidbody.velocity.y);
		if (facingRight){
			//AddForce (new Vector2 (0, 5000), ForceMode.Acceleration);
			//AddForce (new Vector2 (200, 0), ForceMode.VelocityChange);
			StartCoroutine (applyJumpWallForce(19, 1, 12f));
			//AddForce (new Vector2 (jumpForce*3, jumpForce*1.5f), ForceMode.Impulse);
		}
		else {
			//AddForce (new Vector2 (0, 5000), ForceMode.Acceleration);
			//AddForce (new Vector2 (-200, 0), ForceMode.VelocityChange);
			StartCoroutine (applyJumpWallForce(19, -1, 	12f));
			//AddForce (new Vector2 (-jumpForce*3, jumpForce*1.5f), ForceMode.Impulse);
		}
		blockJumpMovement = true;
	}

	private IEnumerator applyJumpWallForce(int frameCount, int dir, float jumpForce){
		float verticalForce = jumpForce;
		//StartCoroutine (stunPlayer (frameCount));
		while (frameCount > 0)
		{
			frameCount--;
			jumpForce-=0.5f;
			if (jumpForce < 1)
				jumpForce = 1;
			AddForce (new Vector2 (dir*verticalForce, jumpForce), ForceMode.Impulse);
			yield return null;
		}
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

    //Boosts the player in the direction he is facing
    public void BoostPlayer(Vector2 force)
    {
        if (facingRight)
            force.x = Mathf.Abs(force.x);
        else
            force.x = Mathf.Abs(force.x) * -1;

        AddForce(force, ForceMode.Force);
    }

    public void AddPowerUp(IPowerUp powerUp)
    {
        currentPowerUp = powerUp;
        powerUp.Setup(this);
    }

    public void RemovePowerUp()
    {
        currentPowerUp = null;
    }

    public void StunPlayer()
    {
        StartCoroutine(stunPlayer(15));
    }
}

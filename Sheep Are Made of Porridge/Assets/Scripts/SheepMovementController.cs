using UnityEngine;
using System.Collections;

public enum MoveState
{
	Idle,
	Walk,
	Run
}

public class SheepMovementController : MonoBehaviour
{
	public float hMaxSpeed;
	public float hStopForce;
	public MoveState moveState;

	public float walkForce;
	public float walkMaxSpeed;
	public float runForce;
	public float runMaxSpeed;
	public float jumpHeight;

	public bool isGrounded;
	public float groundCheckRadius;
	public LayerMask groundLayer;

	private Rigidbody2D rigidbodyLocal;
	private Animator animatorLocal;
	private float jumpVelocity;

	void HorizontalMovement()
	{
		float hInput = Input.GetAxis("Horizontal");
		bool runKey = Input.GetKey(KeyCode.LeftShift);

		if (Mathf.Abs(hInput) < 0.01f)
		{
			if (Mathf.Abs(rigidbodyLocal.velocity.x) < 0.01f)
				moveState = MoveState.Idle;
			else
				rigidbodyLocal.AddForce(
					new Vector2(-rigidbodyLocal.velocity.x * hStopForce, 0), ForceMode2D.Impulse);
			animatorLocal.SetFloat("hSpeed", rigidbodyLocal.velocity.x / runMaxSpeed);
			return;
		}

		float hForce, hMaxSpeed;

		moveState = (runKey == true) ? MoveState.Run : MoveState.Walk;

		if (moveState == MoveState.Run)
		{
			hForce = runForce;
			hMaxSpeed = runMaxSpeed;
		}
		else
		{
			hForce = walkForce;
			hMaxSpeed = walkMaxSpeed;
		}

		rigidbodyLocal.AddForce(new Vector2(hInput * hForce, 0));

		//	Max speed clamp
		if (Mathf.Abs(rigidbodyLocal.velocity.x) > hMaxSpeed)
		{
			rigidbodyLocal.velocity
				= new Vector2(
					Mathf.Clamp(rigidbodyLocal.velocity.x, -hMaxSpeed, hMaxSpeed),
					rigidbodyLocal.velocity.y);
		}
		animatorLocal.SetFloat("hSpeed", rigidbodyLocal.velocity.x / runMaxSpeed);
	}

	void JumpMovement()
	{
		isGrounded = Physics2D.OverlapCircle(this.transform.position, groundCheckRadius, groundLayer);
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rigidbodyLocal.velocity = new Vector2(rigidbodyLocal.velocity.x, jumpVelocity);
			isGrounded = false;
		}
		animatorLocal.SetBool("isGrounded", isGrounded);
		animatorLocal.SetFloat("vSpeedPerc", rigidbodyLocal.velocity.y/jumpVelocity);
	}

	//	------	Unity callback functions
	void Start()
	{
		rigidbodyLocal = GetComponent<Rigidbody2D>();
		animatorLocal = GetComponent<Animator>();
		jumpVelocity = Mathf.Sqrt(2f * jumpHeight * -Physics2D.gravity.y);
	}

	void FixedUpdate()
	{
		HorizontalMovement();
		JumpMovement();
	}
}

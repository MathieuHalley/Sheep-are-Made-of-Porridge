using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float movementImpulse = 4f;
	public float jumpImpulse = 10f;
	public float maxMoveVel;
	public float movementImpulsePushOffMultiplier;
	public float movementDecelerationMultiplier;
	public int xAxisMoveDir;
	public int xAxisInputDir;
	public float minMoveVelX = 0.2f;
	public bool isGrounded = true;

	private Rigidbody2D rigidbody2D;
	private Collider2D collider2D;

	private void Awake()
	{	
		rigidbody2D = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>() ?? gameObject.AddComponent<Collider2D>();
	}

	private void Update()
	{
		xAxisMoveDir = Mathf.RoundToInt(Mathf.Clamp(rigidbody2D.velocity.x,-1,1));
		xAxisInputDir = (int)Input.GetAxis("Horizontal");

		//Move Socrates on x-axis
		if ( xAxisInputDir != 0 )
		{
			XAxisMove();
		}
		else if ( xAxisInputDir == 0 && rigidbody2D.velocity.x != 0 && isGrounded )
		{
			XAxisSlowDown();
		}

		if ( Input.GetKeyDown(KeyCode.Space) && isGrounded )
		{
			Jump();
		}
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), 
			"Socrate's Velocity: " + rigidbody2D.velocity.ToString()  + " (" + rigidbody2D.velocity.magnitude.ToString() + ")\n" +
			"MoveDir: " + xAxisMoveDir + "  InputDir: " + Input.GetAxis("Horizontal"));
	}

	private void XAxisMove()
	{
		float xAxisMovementForce;
		float xAxisInputValue = Input.GetAxis("Horizontal");

		xAxisMovementForce 
			= xAxisInputValue 
			* movementImpulse 
			* (( xAxisMoveDir != xAxisInputDir ) ? movementImpulsePushOffMultiplier : 1f);

		rigidbody2D.AddForce(Vector2.right * xAxisMovementForce * Time.deltaTime, ForceMode2D.Impulse);

		if ( Mathf.Abs(rigidbody2D.velocity.x) > maxMoveVel )
		{
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxMoveVel, rigidbody2D.velocity.y);
		}
	}

	private void XAxisSlowDown()
	{
		float newVelX = rigidbody2D.velocity.x * movementDecelerationMultiplier;

		if ( Mathf.Abs(newVelX) < minMoveVelX )
		{
			newVelX = 0f;
		}
		rigidbody2D.velocity = new Vector2(newVelX, rigidbody2D.velocity.y);
	}

	private void Jump()
	{
		isGrounded = false;
		rigidbody2D.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);		
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if ( col.gameObject.tag == "Ground" )
		{
			isGrounded = true;
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerKinemeticMovement : MonoBehaviour 
{
	public float moveSpeed;
	public int maxMoveSpeed;	//	Maximum velocity
	public float moveAccel;	//	Acceleration per second (-1 = infinite acceleration)
	public float jumpImpulse;	//	Upward force when the player jumps

	public Vector2 newVelocity;

	private Rigidbody2D rigidbody2D;
	private Collider2D collider2D;
	private bool playerMoved = false;

	private void Awake()
	{
		rigidbody2D = 
			this.GetComponent<Rigidbody2D>() ?? 
			this.gameObject.AddComponent<Rigidbody2D>();
		collider2D =
			this.GetComponent<Collider2D>() ??
			this.gameObject.AddComponent<Collider2D>();
		playerMoved = false;
	}

	private void Update()
	{
		MovePlayer();
	
	}

	private void FixedUpdate()
	{
		if ( playerMoved == true )
		{
			this.rigidbody2D.velocity = newVelocity;
			playerMoved = false;
		}
	}

	private void MovePlayer()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float newXPosition;
//		moveSpeed *= moveAccel
		if ( moveAccel == -1 )
		{
			moveSpeed += horizontalInput * maxMoveSpeed * Time.deltaTime;
		}
		else
		{
			moveSpeed += horizontalInput * moveAccel * Time.deltaTime;
		}

		moveSpeed = Mathf.Clamp(moveSpeed, -maxMoveSpeed, maxMoveSpeed);
		this.transform.Translate(moveSpeed,0,0);
		newVelocity = rigidbody2D.velocity + new Vector2(moveSpeed, 0);
		playerMoved = true;
	}

}

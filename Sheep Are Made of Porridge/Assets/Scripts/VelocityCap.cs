using UnityEngine;
using System.Collections;

public class VelocityCap : MonoBehaviour {


	public bool limitXVel = true;
	public bool limitYVel = true;
	public Vector2 maxMoveVel;

	private Rigidbody2D rigidbody2D;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
	}

	void Update () {
		float newVelX = rigidbody2D.velocity.x;
		float newVelY = rigidbody2D.velocity.y;

		if ( limitXVel == true && Mathf.Abs(rigidbody2D.velocity.x) > maxMoveVel.x )
		{
			newVelX = Mathf.Sign(rigidbody2D.velocity.x) * maxMoveVel.x;
		}
		if ( limitYVel == true && Mathf.Abs(rigidbody2D.velocity.y) > maxMoveVel.y )
		{
			newVelY = Mathf.Sign(rigidbody2D.velocity.y) * maxMoveVel.y;
		}

		if ( newVelX != rigidbody2D.velocity.x || newVelY != rigidbody2D.velocity.y )
		{
			rigidbody2D.velocity = new Vector2(newVelX,newVelY);
		}

	}
}

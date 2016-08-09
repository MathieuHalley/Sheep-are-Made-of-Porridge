using UnityEngine;
using System;

[System.Serializable]
public class GroundUnitMovementParameters : UnitMovementParameters
{
	public float jumpHeight;
}


public class UnitJumpEventArgs : EventArgs { }


public interface IGroundUnitMovementModel : IUnitMovementModel
{
	event EventHandler<UnitJumpEventArgs> JumpEvent;

	void InitializeJumpVelocity();
	void JumpMovement();
}


public class GroundUnitMovementModel : UnitMovementModel, IGroundUnitMovementModel
{
	//	Events
	public event EventHandler<UnitJumpEventArgs> JumpEvent = (sender, e) => { };

	//	Fields
	protected new readonly GroundUnitMovementParameters mParams;
	protected float jumpVelocity;


	//	Constructors
	public GroundUnitMovementModel (GameObject unitGameObject, GroundUnitMovementParameters mParams) : base(unitGameObject, mParams)
	{
		this.mParams = mParams;
	}

	//	Functions
	public void InitializeJumpVelocity()
	{
		jumpVelocity = Mathf.Sqrt(2f * mParams.jumpHeight * -Physics2D.gravity.y);
	}

	public override void StopMovement()
	{
		if (Mathf.Abs(unitRigidbody2D.velocity.x) < 0.01f)
		{
			CurMoveState = MoveState.Idle;
		}
		else
		{
			unitRigidbody2D.AddForce(new Vector2(-unitRigidbody2D.velocity.x * mParams.stopForce, 0), ForceMode2D.Impulse);
		}
		return;
	}

	public override void Movement(Vector2 dir)
	{
		float hForce, hMaxVel;

		if (CurMoveState == MoveState.FastMove)
		{
			hForce = mParams.fastMoveForce;
			hMaxVel = mParams.fastMoveMaxVel;
		}
		else
		{
			hForce = mParams.moveForce;
			hMaxVel = mParams.moveMaxVel;
		}

		unitRigidbody2D.AddForce(new Vector2(dir.x * hForce, 0));

		if (Mathf.Abs(unitRigidbody2D.velocity.x) > hMaxVel)
		{
			unitRigidbody2D.velocity = new Vector2(Mathf.Clamp(unitRigidbody2D.velocity.x, -hMaxVel, hMaxVel), unitRigidbody2D.velocity.y);
		}
	}

	public void JumpMovement()
	{
		if (CheckIsGrounded())
		{
			unitRigidbody2D.velocity = new Vector2(unitRigidbody2D.velocity.x, jumpVelocity);
			OnJumpEvent(new UnitJumpEventArgs());
			IsGrounded = false;
		}
	}

	//	Event Extensions
	protected virtual void OnJumpEvent(UnitJumpEventArgs e)
	{
		EventHandler<UnitJumpEventArgs> handler = JumpEvent;
		if (handler != null)
			handler(this, e);
	}
}

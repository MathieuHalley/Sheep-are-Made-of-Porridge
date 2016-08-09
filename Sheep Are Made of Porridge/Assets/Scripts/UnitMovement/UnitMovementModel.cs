using UnityEngine;
using System;

[System.Serializable]
public class UnitMovementParameters
{
	public float stopForce;
	public float moveForce;
	public float moveMaxVel;
	public float fastMoveForce;
	public float fastMoveMaxVel;
	public float groundCheckRadius;
	public LayerMask groundLayer;
}


public enum MoveState { Idle, Move, FastMove }


public class UnitMovementEventArgs : EventArgs { }
public class UnitIsGroundedChangedEventArgs : EventArgs { }
public class UnitMoveStateChangedEventArgs : EventArgs { }


public interface IUnitMovementModel
{
	event EventHandler<UnitMovementEventArgs> MovementEvent;
	event EventHandler<UnitIsGroundedChangedEventArgs> IsGroundedChangedEvent;
	event EventHandler<UnitMoveStateChangedEventArgs> MoveStateChangedEvent;

	Vector2 Velocity { get; set; }
	bool IsGrounded { get; set; }
	MoveState CurMoveState { get; set; }

	void StopMovement();
	void Movement(Vector2 dir);
	bool CheckIsGrounded();
}


public class UnitMovementModel : IUnitMovementModel
{
	//	Events
	public event EventHandler<UnitMovementEventArgs> MovementEvent = (sender, e) => { };
	public event EventHandler<UnitIsGroundedChangedEventArgs> IsGroundedChangedEvent = (sender, e) => { };
	public event EventHandler<UnitMoveStateChangedEventArgs> MoveStateChangedEvent = (sender, e) => { };

	//	Fields
	protected readonly UnitMovementParameters mParams;
	protected GameObject unitGameObject;
	protected Rigidbody2D unitRigidbody2D;
	protected MoveState curMoveState;
	protected bool isGrounded;

	//	Properties
	public Vector2 Velocity
	{
		get { return unitRigidbody2D.velocity; }
		set
		{
			if (unitRigidbody2D.velocity != value)
			{
				unitRigidbody2D.velocity = value;
				OnMovementEvent(new UnitMovementEventArgs());
			}
		}
	}

	public MoveState CurMoveState
	{
		get { return curMoveState; }
		set
		{
			if (curMoveState != value)
			{
				curMoveState = value;
				OnMoveStateChangedEvent(new UnitMoveStateChangedEventArgs());
			}
		}
	}

	public bool IsGrounded
	{
		get { return isGrounded; }
		set
		{
			if (isGrounded != value)
			{
				isGrounded = value;
				OnIsGroundedChangedEvent(new UnitIsGroundedChangedEventArgs());
			}
		}
	}


	//	Constructors
	public UnitMovementModel (GameObject unitGameObject, UnitMovementParameters mParams)
	{
		this.unitGameObject = unitGameObject;
		this.mParams = mParams;
		unitRigidbody2D = unitGameObject.GetComponent<Rigidbody2D>();
	}


	//	Functions
	public virtual void StopMovement()
	{
		if (unitRigidbody2D.velocity.magnitude < 0.01f)
		{
			CurMoveState = MoveState.Idle;
		}
		else
		{
			unitRigidbody2D.AddForce(
				new Vector2(
					-unitRigidbody2D.velocity.x * mParams.stopForce,
					-unitRigidbody2D.velocity.y * mParams.stopForce),
				ForceMode2D.Impulse);
		}
		return;
	}

	public virtual void Movement(Vector2 dir)
	{
		float force, maxVel;

		if (CurMoveState == MoveState.FastMove)
		{
			force = mParams.fastMoveForce;
			maxVel = mParams.fastMoveMaxVel;
		}
		else
		{
			force = mParams.moveForce;
			maxVel = mParams.moveMaxVel;
		}

		unitRigidbody2D.AddForce(dir * force);

		if (Mathf.Abs(unitRigidbody2D.velocity.x) > maxVel)
			Velocity = Vector2.ClampMagnitude(Velocity, maxVel);
	}

	public bool CheckIsGrounded()
	{
		IsGrounded 
			= Physics2D.OverlapCircle(
				unitGameObject.transform.position, 
				mParams.groundCheckRadius, 
				mParams.groundLayer);

		return IsGrounded;
	}


	//	Event Extensions
	protected virtual void OnMovementEvent(UnitMovementEventArgs e)
	{
		EventHandler<UnitMovementEventArgs> handler = MovementEvent;
		if (handler != null)
			handler(this, e);
	}
	protected virtual void OnIsGroundedChangedEvent(UnitIsGroundedChangedEventArgs e)
	{
		EventHandler<UnitIsGroundedChangedEventArgs> handler = IsGroundedChangedEvent;
		if (handler != null)
			handler(this, e);
	}
	protected virtual void OnMoveStateChangedEvent(UnitMoveStateChangedEventArgs e)
	{
		EventHandler<UnitMoveStateChangedEventArgs> handler = MoveStateChangedEvent;
		if (handler != null)
			handler(this, e);
	}
}

using UnityEngine;
using System;


public class MovementInputEventArgs : EventArgs
{
	public Vector2 MoveDir { get; private set; }
	public bool IsRunning { get; private set; }
	public MovementInputEventArgs(Vector2 moveDir, bool isRunning)
	{
		MoveDir = moveDir;
		IsRunning = isRunning;
	}
}
public class StopMovementInputEventArgs : EventArgs { }


public interface IUnitMovementView
{
	event EventHandler<MovementInputEventArgs> MovementInputEvent;
	event EventHandler<StopMovementInputEventArgs> StopMovementInputEvent;

	Vector2 DirInput { get; set; }
	bool RunInput { get; set; }

	Vector2 Velocity { set; }
	bool IsGrounded { set; }
	MoveState CurMoveState { set; }
}


/// <summary>
///		Takes movement input for units & triggers input events
///		Base class for other UnitMovementView classes
/// </summary>
public abstract class UnitMovementView : MonoBehaviour, IUnitMovementView
{
	//	Events
	public event EventHandler<MovementInputEventArgs> MovementInputEvent = (sender, e) => { };
	public event EventHandler<StopMovementInputEventArgs> StopMovementInputEvent = (sender, e) => { };

	//	Fields
	protected bool runInput;
	protected Vector2 dirInput;
	protected GameObject unitGameObject;
	protected Rigidbody2D unitRigidbody2D;
	protected Animator unitAnimator;

	//	Properties
	public Vector2 DirInput
	{
		get { return dirInput; }
		set
		{
			if (dirInput != value)
				dirInput = value;
		}
	}

	public bool RunInput
	{
		get { return runInput; }
		set
		{
			if (runInput != value)
				runInput = value;
		}
	}

	public Vector2 Velocity
	{
		set
		{
			if (unitRigidbody2D.velocity != value)
			{
				unitRigidbody2D.velocity = value;
				unitAnimator.SetFloat("hVelocity", value.x);
				unitAnimator.SetFloat("vVelocity", value.y);
			}
		}
	}

	public bool IsGrounded { set { unitAnimator.SetBool("isGrounded", value); } }
	public MoveState CurMoveState { protected get; set; }


	//	Functions
	protected void Awake()
	{
		unitGameObject = this.gameObject;
		unitRigidbody2D = unitGameObject.GetComponent<Rigidbody2D>();
		unitAnimator = unitGameObject.GetComponent<Animator>();
	}

	protected virtual void Move()
	{
		if (DirInput.magnitude >= 0.01f || unitRigidbody2D.velocity.magnitude > 0.01f)
		{
			OnMovementInputEvent(new MovementInputEventArgs(DirInput, RunInput));
		}
		else if (DirInput.magnitude < 0.01f && CurMoveState != MoveState.Idle)
		{
			OnStopMovementInputEvent(new StopMovementInputEventArgs());
		}
	}

	protected virtual void FixedUpdate()
	{
		Move();
	}

	//	Event Extensions
	protected virtual void OnMovementInputEvent(MovementInputEventArgs e)
	{
		EventHandler<MovementInputEventArgs> handler = MovementInputEvent;
		if (handler != null)
			handler(this, e);
	}

	protected virtual void OnStopMovementInputEvent(StopMovementInputEventArgs e)
	{
		EventHandler<StopMovementInputEventArgs> handler = StopMovementInputEvent;
		if (handler != null)
			handler(this, e);
	}
}

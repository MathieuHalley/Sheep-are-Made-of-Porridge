using UnityEngine;
using System;


public class JumpInputEventArgs : EventArgs { }


public interface IGroundUnitMovementView : IUnitMovementView
{
	event EventHandler<JumpInputEventArgs> JumpInputEvent;
	bool JumpInput { get; set; }
}


/// <summary>
///		Takes movement input for ground units & triggers input events
/// </summary>
public class GroundUnitMovementView : UnitMovementView, IGroundUnitMovementView
{
	//	Events
	public event EventHandler<JumpInputEventArgs> JumpInputEvent = (sender, e) => { };

	//	Fields
	protected bool jumpInput;

	//	Properties
	public bool JumpInput
	{
		get { return jumpInput; }
		set
		{
			if (jumpInput != value)
				jumpInput = value;
		}
	}


	//	Functions
	protected override void Move()
	{
		if (Mathf.Abs(DirInput.x) >= 0.01f || Mathf.Abs(unitRigidbody2D.velocity.x) > 0.01f)
		{
			OnMovementInputEvent(new MovementInputEventArgs(DirInput, RunInput));
		}
		else if (Mathf.Abs(DirInput.x) < 0.01f && CurMoveState != MoveState.Idle)
		{
			OnStopMovementInputEvent(new StopMovementInputEventArgs());
		}
	}

	protected virtual void Jump()
	{
		if (JumpInput)
		{
			OnJumpInputEvent(new JumpInputEventArgs());
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Jump();
	}

	//	Event Extensions
	protected virtual void OnJumpInputEvent(JumpInputEventArgs e)
	{
		EventHandler<JumpInputEventArgs> handler = JumpInputEvent;
		if (handler != null)
			handler(this, e);
	}
}

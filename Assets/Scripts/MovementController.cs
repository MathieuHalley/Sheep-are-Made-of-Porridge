﻿using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MovementController : ReactiveController<MovementControllerData>
{
	public ReactiveProperty<bool> IsGrounded { get; private set; }
	public ReactiveProperty<bool> IsJumping { get; private set; }

	private void Awake()
	{
		IsGrounded = new ReactiveProperty<bool>(false);
		IsJumping = new ReactiveProperty<bool>(false);
	}

	private void Start()
	{
		GroundCollisionEnterSubscription();
		GroundCollisionExitSubscription();
	}

	public void ProcessMovementInput(Vector2 movementInput)
	{
		Vector2 movementForce = Vector2.right;
		float maxVelocity = Data.MovementParameters.MaxVelocity;

		movementForce *= movementInput.x != 0
			? Mathf.Abs(Data.MovementParameters.AccelerationForce) * movementInput.x
			:-Mathf.Abs(Data.MovementParameters.DecelerationForce) * Rigidbody.velocity.x / maxVelocity;
		Rigidbody.AddForce(movementForce, ForceMode2D.Impulse);
		Rigidbody.velocity = new Vector2(
			Mathf.Clamp(Rigidbody.velocity.x, -maxVelocity, maxVelocity), 
			Rigidbody.velocity.y);
	}

	public void ProcessJumpInput()
	{
		if (!IsGrounded.Value)
			return;
		float jumpForce = Mathf.Sqrt(2f * Data.JumpHeight * -Physics2D.gravity.y * Rigidbody.gravityScale);

		Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

		IsJumping.Value = true;
		IsGrounded.Value = false;
	}

	private bool IsGroundCollision(Collision2D collision)
	{
		int groundLayer = LayerMask.NameToLayer(Data.GroundLayer);
		if (collision.collider.gameObject.layer != groundLayer)
			return false;
		RaycastHit2D groundCheckHit = 
			Physics2D.Raycast(
				this.transform.position,
				Vector2.down,
				Data.GroundCheckRadius,
				1 << groundLayer);
		return groundCheckHit.collider != null ? true : false;
	}

	private System.IDisposable GroundCollisionEnterSubscription()
	{
		return this
			.OnCollisionEnter2DAsObservable()
			.Where(collision => !IsGrounded.Value && IsGroundCollision(collision))
			.Subscribe(_ =>
			{
				IsGrounded.Value = true;
				IsJumping.Value = false;
			})
			.AddTo(this);
	}
	private System.IDisposable GroundCollisionExitSubscription()
	{
		return this
			.OnCollisionExit2DAsObservable()
			.Where(collision => IsGrounded.Value && !IsGroundCollision(collision))
			.Subscribe(_ => IsGrounded.Value = false)
			.AddTo(this);
	}
}

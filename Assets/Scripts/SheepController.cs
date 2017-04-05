using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Assets.Scripts
{
	public class SheepController : ReactiveController<SheepControllerData>
	{
		public bool IsGrounded
		{
			get { return Data.IsGroundedProperty.Value; }
			set { Data.IsGroundedProperty.Value = value; }
		}

		public bool IsJumping
		{
			get { return Data.IsJumpingProperty.Value; }
			set { Data.IsJumpingProperty.Value = value; }
		}

		public bool IsPorridge
		{
			get { return Data.IsPorridgeProperty.Value; }
			set
			{
				Data.IsPorridgeProperty.Value = value;
				Data.PorridgeBody.SetActive(value);
				Data.PorridgeCollider.enabled = value;
				Data.SheepBody.SetActive(!value);
				Data.SheepCollider.enabled = !value;
			}
		}

		private void Start()
		{
			GroundCollisionEnterSubscription();
			GroundCollisionExitSubscription();
		}

		public void ProcessMovementInput(Vector2 movementInput)
		{
			var maxVelocity = Data.MovementParameters.MaxVelocity;
			var movementForce = Vector2.right;
			movementForce *= Mathf.Abs(movementInput.x) > 0.05f
				? Mathf.Abs(Data.MovementParameters.AccelerationForce) * movementInput.x
				: -Mathf.Abs(Data.MovementParameters.DecelerationForce) * Rigidbody.velocity.x / maxVelocity;
			Rigidbody.AddForce(movementForce, ForceMode2D.Impulse);
			Rigidbody.velocity = new Vector2(
				Mathf.Clamp(Rigidbody.velocity.x, -maxVelocity, maxVelocity),
				Rigidbody.velocity.y);
		}

		public void ProcessJumpInput()
		{
			if (!IsGrounded) return;
			var jumpForce = Mathf.Sqrt(2f * Data.JumpHeight * -Physics2D.gravity.y * Rigidbody.gravityScale);
			IsJumping = true;
			IsGrounded = false;
			Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		}

		private bool IsGroundCollision(Collision2D collision)
		{
			var groundLayer = LayerMask.NameToLayer(Data.GroundLayer);
			if (collision.collider.gameObject.layer != groundLayer) return false;
			var groundCheckHit = Physics2D.Raycast(
				CurPosition,
				Vector2.down,
				Data.GroundCheckRadius,
				1 << groundLayer);
			return groundCheckHit.collider != null;
		}

		private void GroundCollisionEnterSubscription()
		{
			this.OnCollisionEnter2DAsObservable()
				.Where(collision => !IsGrounded && IsGroundCollision(collision))
				.Subscribe(_ =>
				{
					IsGrounded = true;
					IsJumping = false;
				})
				.AddTo(this);
		}

		private void GroundCollisionExitSubscription()
		{
			this.OnCollisionExit2DAsObservable()
				.Where(collision => IsGrounded && !IsGroundCollision(collision))
				.Subscribe(_ => IsGrounded = false)
				.AddTo(this);
		}
	}
}
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class MovementController : MonoBehaviour
{
	[SerializeField]
	private MovementControllerData _data;
	private Rigidbody2D _rigidbody;
	private MovementControllerData Data { get { return _data; } }

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		GroundCollisionEnterSubscription();
		GroundCollisionExitSubscription();
	}

	public void ProcessMovementInput(Vector2 movementInput)
	{
		MovementParameters movementParams = Data.MovementParameters;
		Vector2 movementForce = Vector2.right;
		float maxVelocity = movementParams.MaxVelocity;
		float clampedVelocityX;

		movementForce *= movementInput.x != 0
			? Mathf.Abs(movementParams.AccelerationForce) * movementInput.x
			:-Mathf.Abs(movementParams.DecelerationForce) * _rigidbody.velocity.x / maxVelocity;
		_rigidbody.AddForce(movementForce, ForceMode2D.Impulse);
		clampedVelocityX = Mathf.Clamp(_rigidbody.velocity.x, -maxVelocity, maxVelocity);
		_rigidbody.velocity = new Vector2(clampedVelocityX, _rigidbody.velocity.y);

		Data.MovementVelocity = _rigidbody.velocity;
	}

	public void ProcessJumpInput(Unit _)
	{
		if (!Data.IsGrounded)
			return;
		float jumpForce = Mathf.Sqrt(2f * Data.JumpHeight * -Physics2D.gravity.y * _rigidbody.gravityScale);

		_rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

		Data.IsJumping = true;
		Data.IsGrounded = false;
		Data.MovementVelocity = _rigidbody.velocity;
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

	private IDisposable GroundCollisionEnterSubscription()
	{
		return this
			.OnCollisionEnter2DAsObservable()
			.Where(collision => Data.IsGrounded == false && IsGroundCollision(collision) == true)
			.Subscribe(_ =>
			{
				Data.IsGrounded = true;
				Data.IsJumping = false;
			})
			.AddTo(this);
	}
	private IDisposable GroundCollisionExitSubscription()
	{
		return this
			.OnCollisionExit2DAsObservable()
			.Where(collision => Data.IsGrounded == true && IsGroundCollision(collision) == false)
			.Subscribe(_ => Data.IsGrounded = false)
			.AddTo(this);
	}
}

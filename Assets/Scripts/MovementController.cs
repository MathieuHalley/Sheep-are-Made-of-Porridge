using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MovementController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D _rigidbody;
	[SerializeField]
	private MovementControllerData _movementControllerData;
	
	public MovementControllerData Data { get { return _movementControllerData; } }

	private void Start()
	{
		this.OnCollisionEnter2DAsObservable()
			.Where(collision => Data.IsGrounded == false && IsGroundCollision(collision) == true)
			.TakeUntilDestroy(this)
			.Subscribe(OnGroundCollisionEnter);
		this.OnCollisionExit2DAsObservable()
			.Where(collision => Data.IsGrounded == true && IsGroundCollision(collision) == false)
			.TakeUntilDestroy(this)
			.Subscribe(OnGroundCollisionEnter);
	}

	public void ProcessMovementInput(Vector2 movementInput)
	{
		MovementParameters movementParameters = Data.StandardMovementParameters;
		Data.IsMoving = movementInput.x != 0 ? true : false;
		Vector2 movementForce = Data.IsMoving
			?  Mathf.Abs(movementParameters.AccelerationForce) * Vector2.right * movementInput.x
			: -Mathf.Abs(movementParameters.DecelerationForce) * Vector2.right * _rigidbody.velocity.x;
		_rigidbody.AddForce(movementForce);
		_rigidbody.velocity = new Vector2(Mathf.Clamp(_rigidbody.velocity.x, -movementParameters.MaxVelocity, movementParameters.MaxVelocity), _rigidbody.velocity.y);
		Data.MovementVelocity = _rigidbody.velocity;
	}

	public void ProcessJumpInput(bool jumpInput)
	{
		float jumpForce = Mathf.Sqrt(2f * Data.JumpHeight * -Physics2D.gravity.y);
		_rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		Data.IsJumping = true;
		Data.IsGrounded = false;
	}

	private bool IsGroundCollision(Collision2D collision)
	{
		int groundLayer = LayerMask.NameToLayer(Data.GroundLayer);
		if (collision.collider.gameObject.layer != groundLayer)
			return false;
		return Physics2D.Raycast(this.transform.position, Vector2.down, Data.GroundCheckRadius, 1 << groundLayer).collider != null
			? true : false;
	}

	private void OnGroundCollisionEnter(Collision2D collision)
	{
		Data.IsGrounded = true;
		Data.IsJumping = false;
	}

	private void OnGroundCollisionExit(Collision2D collision)
	{
		Data.IsGrounded = false;
	}
}

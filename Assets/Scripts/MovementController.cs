using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MovementController : MonoBehaviour
{
	[SerializeField]
	private MovementControllerData _movementControllerData;
	private Rigidbody2D _rigidbody;
	private MovementControllerData Data { get { return _movementControllerData; } }

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		this.OnCollisionEnter2DAsObservable()
			.Where(collision => Data.IsGrounded == false && IsGroundCollision(collision) == true)
			.Subscribe(_ =>
			{
				Data.IsGrounded = true;
				Data.IsJumping = false;
			}).AddTo(this);
		this.OnCollisionExit2DAsObservable()
			.Where(collision => Data.IsGrounded == true && IsGroundCollision(collision) == false)
			.Subscribe(_ =>
			{
				Data.IsGrounded = false;
			}).AddTo(this);
	}

	public void ProcessMovementInput(Vector2 movementInput)
	{
		MovementParameters movementParameters = Data.StandardMovementParameters;
		Vector2 movementForce = Vector2.right;
		movementForce *= movementInput.x != 0
			? Mathf.Abs(movementParameters.AccelerationForce) * movementInput.x
			:-Mathf.Abs(movementParameters.DecelerationForce) * _rigidbody.velocity.x / movementParameters.MaxVelocity;
		_rigidbody.AddForce(movementForce, ForceMode2D.Impulse);
		_rigidbody.velocity = new Vector2(
			Mathf.Clamp(_rigidbody.velocity.x, -movementParameters.MaxVelocity, movementParameters.MaxVelocity), 
			_rigidbody.velocity.y
		);
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
}

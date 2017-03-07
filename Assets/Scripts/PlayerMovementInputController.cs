using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMovementInputController : MonoBehaviour
{
	[SerializeField]
	private Rigidbody2D _rigidbody;
	[SerializeField]
	private MovementControllerData _movementControllerData;

//	private IObservable<Vector2> movement;

	private void Start()
	{
		this.FixedUpdateAsObservable()
			.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
			.Subscribe(ProcessMovementInput);

		this.FixedUpdateAsObservable()
			.Where(_ => Input.GetKeyDown(KeyCode.Space) == true && _movementControllerData.IsGrounded == true)
			.Select(_ => true)
			.Subscribe(ProcessJumpInput);

		this.FixedUpdateAsObservable()
			.Where(_ => Mathf.Approximately(_rigidbody.velocity.y, 0f))
			.Subscribe(_ => { _movementControllerData.IsGrounded = true; _movementControllerData.IsJumping = false; });
	}

	private void ProcessMovementInput(Vector2 movementInput)
	{
		MovementParameters currentMovementParameters = _movementControllerData.StandardMovementParameters;

		if (_movementControllerData.IsFastMoving && !_movementControllerData.IsSlowMoving)
			currentMovementParameters = _movementControllerData.FastMovementParameters;
		else if (!_movementControllerData.IsFastMoving && _movementControllerData.IsSlowMoving)
			currentMovementParameters = _movementControllerData.SlowMovementParameters;

		_rigidbody.AddForce(new Vector2(movementInput.x, 0) * currentMovementParameters.Force);
		Mathf.Clamp(_rigidbody.velocity.x, -currentMovementParameters.MaxVelocity, currentMovementParameters.MaxVelocity);

		_movementControllerData.IsMoving = (movementInput.magnitude > 0) ? true : false;
	}

	private void ProcessJumpInput(bool jumpInput)
	{
		_movementControllerData.IsJumping = true;
		_movementControllerData.IsGrounded = false;
		float jumpForce = Mathf.Sqrt(2f * _movementControllerData.JumpHeight * -Physics2D.gravity.y);
		_rigidbody.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
	}
}

using UnityEngine;
using UniRx;

public class MovementControllerEvents : MonoBehaviour
{

	[SerializeField]
	private MovementControllerData _movementControllerData;

	private Vector3 previousMovementDirection;
	private void Start()
	{
		_movementControllerData.MovementVelocityProperty
			.TakeUntilDestroy(this)
			.Subscribe(OnMovementVelocityChanged);
		_movementControllerData.IsGroundedProperty
			.TakeUntilDestroy(this)
			.Subscribe(OnIsGroundedChanged);
		_movementControllerData.IsJumpingProperty
			.TakeUntilDestroy(this)
			.Subscribe(OnIsJumpingChanged);
		_movementControllerData.IsFastMovingProperty
			.TakeUntilDestroy(this)
			.Subscribe(OnIsFastMovingChanged);
		_movementControllerData.IsSlowMovingProperty
			.TakeUntilDestroy(this)
			.Subscribe(OnIsSlowMovingChanged);
	}

	private void OnMovementVelocityChanged(Vector3 newMovementVelocity)
	{
		_movementControllerData.IsMoving = (newMovementVelocity.magnitude > 0) ? true : false;
	}

	private void OnIsGroundedChanged(bool newIsGrounded)
	{
		if (newIsGrounded && _movementControllerData.IsJumping)
			_movementControllerData.IsJumping = false;
	}

	private void OnIsJumpingChanged(bool newIsJumping)
	{
		if (newIsJumping && _movementControllerData.IsGrounded)
			_movementControllerData.IsGrounded = false;
	}

	private void OnIsFastMovingChanged(bool newIsFastMoving)
	{
		if (newIsFastMoving && _movementControllerData.IsSlowMoving)
			_movementControllerData.IsSlowMoving = false;
	}

	private void OnIsSlowMovingChanged(bool newIsSlowMoving)
	{
		if (newIsSlowMoving && _movementControllerData.IsFastMoving)
			_movementControllerData.IsFastMoving = false;
	}
}

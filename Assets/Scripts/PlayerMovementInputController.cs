using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMovementInputController : MonoBehaviour
{
	[SerializeField]
	private MovementController _movementController;

	public MovementController Controller { get { return _movementController; } }

	private void Start()
	{
		var fixedUpdate = this.FixedUpdateAsObservable();
		fixedUpdate
			.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
			.Subscribe(_movementController.ProcessMovementInput);
		fixedUpdate
			.Where(_ => Input.GetKeyDown(KeyCode.Space) && _movementController.Data.IsGrounded)
			.Select(_ => true)
			.Subscribe(_movementController.ProcessJumpInput);
		fixedUpdate
			.Where(_ => Input.GetKeyDown(KeyCode.LeftShift) && !_movementController.Data.IsFastMoving)
			.Select(_ => true)
			.Subscribe(_movementController.ProcessFastMovementToggleInput);
		fixedUpdate
			.Where(_ => Input.GetKeyUp(KeyCode.LeftShift) && _movementController.Data.IsFastMoving)
			.Select(_ => false)
			.Subscribe(_movementController.ProcessFastMovementToggleInput);
		fixedUpdate
			.Where(_ => Input.GetAxis("Vertical") < 0f && !_movementController.Data.IsSlowMoving)
			.Select(_ => true)
			.Subscribe(_movementController.ProcessSlowMovementToggleInput);
		fixedUpdate
			.Where(_ => Input.GetAxis("Vertical") == 0f && _movementController.Data.IsSlowMoving)
			.Select(_ => false)
			.Subscribe(_movementController.ProcessSlowMovementToggleInput);
	}
}

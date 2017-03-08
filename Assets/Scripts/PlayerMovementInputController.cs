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
		this.FixedUpdateAsObservable()
			.Where(_ => Input.GetAxis("Horizontal") != 0 || _movementController.Data.MovementVelocity.x != 0)
			.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
			.Subscribe(_movementController.ProcessMovementInput);
		this.FixedUpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.Space) && _movementController.Data.IsGrounded)
			.Select(_ => true)
			.Subscribe(_movementController.ProcessJumpInput);
	}
}

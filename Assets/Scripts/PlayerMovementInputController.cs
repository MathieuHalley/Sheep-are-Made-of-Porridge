using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMovementInputController : MonoBehaviour
{
	[SerializeField]
	private MovementController _movementController;
	private Rigidbody2D _rigidbody;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		this.FixedUpdateAsObservable()
			.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
			.Where(v => v.x != 0 || _rigidbody.velocity.x != 0)
			.Subscribe(_movementController.ProcessMovementInput);
		this.FixedUpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.Space))
			.Subscribe(_movementController.ProcessJumpInput);
	}
}

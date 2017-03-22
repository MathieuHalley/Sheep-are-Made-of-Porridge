using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerMovementInput : MonoBehaviour
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
		MovementInputSubscription();
		JumpInputSubscription();
	}

	private System.IDisposable MovementInputSubscription()
	{
		return 
		this.FixedUpdateAsObservable()
			.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
			.Where(v => v.x != 0 || _rigidbody.velocity.x != 0)
			.Subscribe(v => _movementController.ProcessMovementInput(v))
			.AddTo(this);
	}

	private System.IDisposable JumpInputSubscription()
	{
		return 
		this.FixedUpdateAsObservable()
			.Where(_ => Input.GetKey(KeyCode.Space))
			.Subscribe(_ => _movementController.ProcessJumpInput())
			.AddTo(this);
	}
}

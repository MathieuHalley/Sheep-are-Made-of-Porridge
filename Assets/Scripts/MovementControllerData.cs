using UnityEngine;
using UniRx;

public class MovementControllerData : MonoBehaviour
{
	[SerializeField]
	private Vector3ReactiveProperty _movementVelocityProperty = new Vector3ReactiveProperty(Vector3.zero);
	[SerializeField]
	private BoolReactiveProperty _isGroundedProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private BoolReactiveProperty _isJumpingProperty = new BoolReactiveProperty(false);
	[SerializeField] 
	private MovementParameters _standardMovementParameters = new MovementParameters();
	[SerializeField]
	private float _jumpHeight = 1f;
	[SerializeField]
	private float _groundCheckRadius = 0.1f;
	[SerializeField]
	private string _groundLayer = "Ground";

	public Vector3ReactiveProperty MovementVelocityProperty { get { return _movementVelocityProperty; } }
	public BoolReactiveProperty IsGroundedProperty { get { return _isGroundedProperty; } }
	public BoolReactiveProperty IsJumpingProperty { get { return _isJumpingProperty; } }
	public MovementParameters StandardMovementParameters { get { return _standardMovementParameters; } }
	public float JumpHeight { get { return _jumpHeight; } }
	public float GroundCheckRadius { get { return _groundCheckRadius; } }
	public string GroundLayer { get { return _groundLayer; } }
	public Vector3 MovementVelocity
	{
		get { return _movementVelocityProperty.Value; }
		set { _movementVelocityProperty.Value = value; }
	}
	public bool IsGrounded
	{
		get { return _isGroundedProperty.Value; }
		set { _isGroundedProperty.Value = value; }
	}
	public bool IsJumping
	{
		get { return _isJumpingProperty.Value; }
		set { _isJumpingProperty.Value = value; }
	}

	private void Awake()
	{

	}
}

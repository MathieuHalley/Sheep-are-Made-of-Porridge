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
	private BoolReactiveProperty _isMovingProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private BoolReactiveProperty _isFastMovingProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private BoolReactiveProperty _isSlowMovingProperty = new BoolReactiveProperty(false);
	[SerializeField] 
	private MovementParameters _fastMovementParameters = new MovementParameters();
	[SerializeField]
	private MovementParameters _slowMovementParameters = new MovementParameters();
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
	public BoolReactiveProperty IsMovingProperty { get { return _isMovingProperty; } }
	public BoolReactiveProperty IsFastMovingProperty { get { return _isFastMovingProperty; } }
	public BoolReactiveProperty IsSlowMovingProperty { get { return _isSlowMovingProperty; } }
	public MovementParameters FastMovementParameters { get { return _fastMovementParameters; } }
	public MovementParameters SlowMovementParameters { get { return _slowMovementParameters; } }
	public MovementParameters StandardMovementParameters { get { return _standardMovementParameters; } }
	public float JumpHeight { get { return _jumpHeight; } }
	public float GroundCheckRadius { get { return _groundCheckRadius; } }
	public string GroundLayer { get { return _groundLayer; } }
	public Vector3 MovementDirection { get { return _movementVelocityProperty.Value.normalized; } }
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
	public bool IsMoving
	{
		get { return _isMovingProperty.Value; }
		set { _isMovingProperty.Value = value; }
	}
	public bool IsFastMoving
	{
		get { return _isFastMovingProperty.Value; }
		set { _isFastMovingProperty.Value = value; }
	}
	public bool IsSlowMoving
	{
		get { return _isSlowMovingProperty.Value; }
		set { _isSlowMovingProperty.Value = value; }
	}
}

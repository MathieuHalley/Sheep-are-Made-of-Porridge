using UnityEngine;
using UniRx;

[System.Serializable]
public class MovementControllerData : ReactiveControllerData
{
	[SerializeField]
	private BoolReactiveProperty _isGroundedProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private BoolReactiveProperty _isJumpingProperty = new BoolReactiveProperty(false);
	[SerializeField] 
	private MovementParameters _movementParameters = new MovementParameters();
	[SerializeField]
	private float _jumpHeight = 1f;
	[SerializeField]
	private float _groundCheckRadius = 0.1f;
	[SerializeField]
	private string _groundLayer = "Ground";

	public BoolReactiveProperty IsGroundedProperty { get { return _isGroundedProperty; } }
	public BoolReactiveProperty IsJumpingProperty { get { return _isJumpingProperty; } }
	public MovementParameters MovementParameters { get { return _movementParameters; } }
	public float JumpHeight { get { return _jumpHeight; } }
	public float GroundCheckRadius { get { return _groundCheckRadius; } }
	public string GroundLayer { get { return _groundLayer; } }
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
}

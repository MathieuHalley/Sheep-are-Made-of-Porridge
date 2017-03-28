using UnityEngine;
using UniRx;

[System.Serializable]
public class MovementControllerData : ReactiveControllerData
{
	[SerializeField]
	private float _jumpHeight = 1f;
	[SerializeField]
	private float _groundCheckRadius = 0.1f;
	[SerializeField]
	private string _groundLayer = "Ground";
	[SerializeField]
	private MovementParameters _movementParameters;

	public MovementParameters MovementParameters { get { return _movementParameters; } }

	public float JumpHeight { get { return _jumpHeight; } }
	public float GroundCheckRadius { get { return _groundCheckRadius; } }
	public string GroundLayer { get { return _groundLayer; } }
}

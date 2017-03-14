using UnityEngine;
using UniRx;

public class CrowControllerData : MonoBehaviour, IReactiveControllerData
{
	[SerializeField]
	private Vector2ReactiveProperty _targetProperty = new Vector2ReactiveProperty(Vector2.zero);
	[SerializeField]
	private BoolReactiveProperty _isActiveProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private MovementParameters _movementParameters = new MovementParameters();
	[SerializeField]
	private AnimationCurve _flightHeight = new AnimationCurve();
	private Vector2 _nest;

	public Vector2ReactiveProperty TargetProperty { get { return _targetProperty; } }
	public BoolReactiveProperty IsActiveProperty { get { return _isActiveProperty; } }
	public MovementParameters MovementParameters { get { return _movementParameters; } }
	public Vector2 Nest
	{
		get { return _nest; }
		set { _nest = value; }
	}
	public Vector2 Target
	{
		get { return _targetProperty.Value; }
		set { _targetProperty.Value = value; }
	}
	public bool IsActive
	{
		get { return _isActiveProperty.Value; }
		set { _isActiveProperty.Value = value; }
	}
	public AnimationCurve FlightHeight { get { return _flightHeight; } }
}

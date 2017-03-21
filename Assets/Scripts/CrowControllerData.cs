using UnityEngine;
using UniRx;


[System.Serializable]
public class CrowControllerData : ReactiveControllerData
{
	[SerializeField]
	private Vector2ReactiveProperty _targetPositionProperty = new Vector2ReactiveProperty(Vector2.zero);
	[SerializeField]
	private BoolReactiveProperty _isActiveProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private MovementParameters _movementParameters = new MovementParameters();
	[SerializeField]
	private AnimationCurve _outwardFlight;
	[SerializeField]
	private AnimationCurve _homewardFlight;
	[SerializeField]
	private FlightCurve _flight;
	private Vector2 _nestPosition;

	public Vector2ReactiveProperty TargetPositionProperty { get { return _targetPositionProperty; } }
	public BoolReactiveProperty IsActiveProperty { get { return _isActiveProperty; } }
	public MovementParameters MovementParameters { get { return _movementParameters; } }
	public Vector2 NestPosition
	{
		get { return _nestPosition; }
		set { _nestPosition = value; }
	}
	public Vector2 TargetPosition
	{
		get { return _targetPositionProperty.Value; }
		set { _targetPositionProperty.Value = value; }
	}
	public bool IsActive
	{
		get { return _isActiveProperty.Value; }
		set { _isActiveProperty.Value = value; }
	}
	public AnimationCurve OutwardFlight { get { return _outwardFlight; } }
	public AnimationCurve HomewardFlight { get { return _homewardFlight; } }
	public FlightCurve Flight
	{
		get { return _flight; }
		set { _flight = value; }
	}
}

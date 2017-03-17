using UnityEngine;
using UniRx;
using System.Collections.Generic;

[System.Serializable]
public class CrowControllerData : ReactiveControllerData
{
	[SerializeField]
	private Vector2ReactiveProperty _targetProperty = new Vector2ReactiveProperty(Vector2.zero);
	[SerializeField]
	private BoolReactiveProperty _isActiveProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private MovementParameters _movementParameters = new MovementParameters();
	[SerializeField]
	private AnimationCurve _outwardFlightCurve = new AnimationCurve();
	[SerializeField]
	private AnimationCurve _homewardFlightCurve = new AnimationCurve();
	private Vector2 _nestPosition;
	private List<Vector3> _flightPath;

	public Vector2ReactiveProperty TargetProperty { get { return _targetProperty; } }
	public BoolReactiveProperty IsActiveProperty { get { return _isActiveProperty; } }
	public MovementParameters MovementParameters { get { return _movementParameters; } }
	public Vector2 NestPosition
	{
		get { return _nestPosition; }
		set { _nestPosition = value; }
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
	public AnimationCurve OutwardFlightCurve { get { return _outwardFlightCurve; } }
	public AnimationCurve HomewardFlightCurve { get { return _homewardFlightCurve; } }
	public List<Vector3> FlightPath
	{
		get { return _flightPath; }
		set { _flightPath = value; }
	}
}

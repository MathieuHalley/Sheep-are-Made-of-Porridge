using UnityEngine;
using UniRx;


[System.Serializable]
public class CrowControllerData : ReactiveControllerData
{
	[SerializeField]
	private MovementParameters _movementParameters = new MovementParameters();
	[SerializeField]
	private float _minFlightHeight;
	[SerializeField]
	private AnimationCurve _outwardFlight;
	[SerializeField]
	private AnimationCurve _homewardFlight;
	[SerializeField]
	private FlightCurve _flight;
	private Transform _sheep;
	private Vector2 _nestPosition;

	public MovementParameters MovementParameters { get { return _movementParameters; } }
	public float MinFlightHeight { get { return _minFlightHeight; } }
	public AnimationCurve OutwardFlight { get { return _outwardFlight; } }
	public AnimationCurve HomewardFlight { get { return _homewardFlight; } }
	public FlightCurve Flight
	{
		get { return _flight; }
		set { _flight = value; }
	}
	public Transform Sheep
	{
		get { return _sheep; }
		set { _sheep = value; }
	}
	public Vector2 NestPosition
	{
		get { return _nestPosition; }
		set { _nestPosition = value; }
	}
	public Bounds NestBounds { get { return new Bounds(_nestPosition, Vector3.one * 0.25f); } }
	public Bounds SheepBounds { get { return new Bounds(_sheep.position, _sheep.localScale); } }
}

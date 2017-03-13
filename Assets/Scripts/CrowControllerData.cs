using UnityEngine;
using UniRx;

public class CrowControllerData : MonoBehaviour
{
	[SerializeField]
	private Vector2ReactiveProperty _targetProperty = new Vector2ReactiveProperty(Vector2.zero);
	[SerializeField]
	private MovementParameters _movementParameters = new MovementParameters();
	private Vector2 _nest;

	public Vector2ReactiveProperty TargetProperty { get { return _targetProperty; } }
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
}

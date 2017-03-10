using UnityEngine;
using UniRx;

public class CrowControllerData : MonoBehaviour
{
	[SerializeField]
	private Vector2ReactiveProperty _targetProperty = new Vector2ReactiveProperty(Vector2.zero);
	[SerializeField]
	private MovementParameters _standardMovementParameters = new MovementParameters();

	public BoolReactiveProperty IsActiveProperty { get; private set; }
	public Vector2ReactiveProperty TargetProperty { get { return _targetProperty; } }
	public MovementParameters StandardMovementParameters { get { return _standardMovementParameters; } }

	public Vector2 Target
	{
		get { return _targetProperty.Value; }
		set { _targetProperty.Value = value; }
	}

	private void Awake()
	{
		IsActiveProperty = _targetProperty
			.Select(t => t != (Vector2)this.gameObject.transform.position)
			.ToReactiveProperty<bool>() 
			as BoolReactiveProperty;
	}
}

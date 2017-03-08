using UnityEngine;
using UniRx;

public class CrowControllerData : MonoBehaviour
{
	[SerializeField]
	BoolReactiveProperty _isSwoopingProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private MovementParameters _standardMovementParameters = new MovementParameters();
	[SerializeField]
	private Vector2 _target = new Vector2();


	public BoolReactiveProperty IsSwoopingProperty { get { return _isSwoopingProperty; } }
	public MovementParameters StandardMovementParameters { get { return _standardMovementParameters; } }

	public bool IsSwooping
	{
		get { return _isSwoopingProperty.Value; }
		set { _isSwoopingProperty.Value = value; }
	}
	public Vector2 Target
	{
		get { return _target; }
		set { _target = value; }
	}
}

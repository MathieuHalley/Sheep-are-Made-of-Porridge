using UnityEngine;
using UniRx;

[System.Serializable]
public class PlayerModeControllerData : ReactiveControllerData
{
	[SerializeField]
	private BoolReactiveProperty _isPorridge = new BoolReactiveProperty(false);
	[SerializeField]
	private Collider2D _porridgeCollider;
	[SerializeField]
	private Collider2D _sheepCollider;
	[SerializeField]
	private GameObject _porridgeBody;
	[SerializeField]
	private GameObject _sheepBody;

	public BoolReactiveProperty IsPorridgeProperty { get { return _isPorridge; } }
	public bool IsPorridge
	{
		get { return _isPorridge.Value; }
		set { _isPorridge.Value = value; }
	}
	public Collider2D PorridgeCollider { get { return _porridgeCollider; } }
	public Collider2D SheepCollider { get { return _sheepCollider; } }
	public GameObject PorridgeBody { get { return _porridgeBody; } }
	public GameObject SheepBody { get { return _sheepBody; } }
}

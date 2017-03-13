using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;

public class CrowTreeControllerData : MonoBehaviour
{
	[SerializeField]
	private BoolReactiveProperty _isSheepInRangeProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private GameObject _crowPrefab;
	[SerializeField]
	private Transform _sheep;
	[SerializeField]
	private Vector2 _nest;
	[SerializeField]
	private int _crowCount = 3;
	[SerializeField]
	private float _crowLaunchDelay = 0.5f;
	private Queue<GameObject> _crowCollection = new Queue<GameObject>(3);

	public BoolReactiveProperty IsSheepInRangeProperty { get { return _isSheepInRangeProperty; } }

	public float CrowLaunchDelay { get { return _crowLaunchDelay; } }
	public Queue<GameObject> CrowCollection { get { return _crowCollection; } }
	public Vector2 Nest { get { return _nest; } }
	public bool IsSheepInRange
	{
		get { return _isSheepInRangeProperty.Value; }
		set { _isSheepInRangeProperty.Value = value; }
	}
	public Transform Sheep
	{
		get { return _sheep; }
		set { _sheep = value; }
	}

	private void Awake()
	{
		if (_nest == Vector2.zero)
			_nest = (Vector2)this.transform.position + Vector2.up;
		for (int i = 0; i < _crowCount; i++)
		{
			GameObject newCrow = Instantiate<GameObject>(
				_crowPrefab,
				this.gameObject.transform.position,
				Quaternion.identity,
				this.transform);
			newCrow
				.GetComponent<CrowController>()
				.SetNest(_nest);
			_crowCollection.Enqueue(newCrow);
		}
	}
}

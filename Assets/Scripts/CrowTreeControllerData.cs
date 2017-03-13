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
	private Transform _sheepTarget;
	[SerializeField]
	private Vector2 _nestTarget;
	private Queue<GameObject> _crowCollection = new Queue<GameObject>(3);

	public BoolReactiveProperty IsSheepInRangeProperty { get { return _isSheepInRangeProperty; } }

	public bool IsSheepInRange
	{
		get { return _isSheepInRangeProperty.Value; }
		set { _isSheepInRangeProperty.Value = value; }
	}

	public Queue<GameObject> CrowCollection { get { return _crowCollection; } }
	public Vector2 NestTarget { get { return _nestTarget; } }
	public Transform SheepTarget
	{
		get { return _sheepTarget; }
		set { _sheepTarget = value; }
	}

	private void Awake()
	{
		if (_nestTarget == Vector2.zero)
			_nestTarget = (Vector2)this.transform.position + Vector2.up;
		for (int i = 0; i < 3; i++)
		{
			GameObject newCrow = Instantiate<GameObject>(
				_crowPrefab,
				this.gameObject.transform.position,
				Quaternion.identity,
				this.transform);
			newCrow.GetComponent<CrowController>().SetCrowTarget(_nestTarget);
			_crowCollection.Enqueue(newCrow);
		}
	}
}

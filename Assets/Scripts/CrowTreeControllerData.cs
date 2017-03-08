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
	private Vector2 _sheepTarget;
	[SerializeField]
	private Vector2 _nestTarget;
	private Queue<GameObject> _crowCollection = new Queue<GameObject>(3);

	public BoolReactiveProperty IsSheepInRangeProperty { get { return _isSheepInRangeProperty; } }

	public bool IsSheepInRange
	{
		get { return _isSheepInRangeProperty.Value; }
		set { _isSheepInRangeProperty.Value = value; }
	}
	public Queue<GameObject> CrowCollection
	{
		get { return _crowCollection; }
		set { _crowCollection = value; }
	}
	public Vector2 SheepTarget
	{
		get { return _sheepTarget; }
		set { _sheepTarget = value; }
	}
	public Vector2 NestTarget
	{
		get { return _nestTarget; }
		set { _nestTarget = value; }
	}

	private void Awake()
	{
		_nestTarget = (Vector2)this.transform.position + Vector2.up;
		for (int i = 0; i < 3; i++)
		{
			GameObject newCrow = Instantiate<GameObject>(
				_crowPrefab,
				this.gameObject.transform.position,
				Quaternion.identity,
				this.transform);
			CrowControllerData newCrowData = newCrow.GetComponent<CrowControllerData>();
			newCrowData.IsSwooping = false;
			_crowCollection.Enqueue(newCrow);
		}
	}
}

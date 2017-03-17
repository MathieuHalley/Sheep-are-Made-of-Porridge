using UnityEngine;
using UniRx;
using System.Collections.Generic;

[System.Serializable]
public class CrowTreeControllerData : ReactiveControllerData
{
	[SerializeField]
	private BoolReactiveProperty _isSheepInRangeProperty = new BoolReactiveProperty(false);
	[SerializeField]
	private GameObject _crowPrefab;
	[SerializeField]
	private Transform _sheep;
	[SerializeField]
	private Vector2 _nestPosition;
	[SerializeField]
	private int _crowCount = 3;
	[SerializeField]
	private float _crowLaunchDelay = 0.5f;
	private Queue<GameObject> _crowCollection = new Queue<GameObject>(3);

	public BoolReactiveProperty IsSheepInRangeProperty { get { return _isSheepInRangeProperty; } }

	public float CrowLaunchDelay { get { return _crowLaunchDelay; } }
	public Queue<GameObject> CrowCollection { get { return _crowCollection; } }
	public Vector2 NestPosition { get { return _nestPosition; } }
	public GameObject CrowPrefab { get { return _crowPrefab; } }
	public int CrowCount { get { return _crowCount; } }
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
}

using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CrowTreeControllerData : ReactiveControllerData
{
	[SerializeField]
	private int _crowCount = 3;
	[SerializeField]
	private float _crowUpdateDelta = 0.5f;
	[SerializeField]
	private GameObject _crowPrefab;
	[SerializeField]
	private GameObject _nest;
	[SerializeField]
	private GameObject _sheep;
	[SerializeField]
	private List<CrowController> _activeCrowCollection = new List<CrowController>(3);
	[SerializeField]
	private List<CrowController> _crowCollection = new List<CrowController>(3);

	private Collider2D _nestCollider;
	private Collider2D _sheepCollider;

	public int CrowCount { get { return _crowCount; } }
	public System.TimeSpan CrowUpdateDelta { get { return System.TimeSpan.FromSeconds(_crowUpdateDelta); } set { _crowUpdateDelta = value.Seconds; } }
	public GameObject CrowPrefab { get { return _crowPrefab; } }
	public GameObject Nest { get { return _nest; } }
	public GameObject Sheep { get { return _sheep; } set { _sheep = value; } }
	public Collider2D NestCollider { get; set; }
	public Collider2D SheepCollider { get; set; }

	public List<CrowController> ActiveCrowCollection { get { return _activeCrowCollection; } }
	public List<CrowController> CrowCollection { get { return _crowCollection; } }
}

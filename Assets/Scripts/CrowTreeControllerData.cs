using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class CrowTreeControllerData : ReactiveControllerData
	{
		private ReactiveProperty<bool> _isChasingSheepProperty;
		private List<CrowController> _crowCollection;
		[SerializeField] [UsedImplicitly] private int _crowCount;
		[SerializeField] [UsedImplicitly] private float _crowLogicUpdateDelta;
		[SerializeField] [UsedImplicitly] private GameObject _crowPrefab;
		[SerializeField] [UsedImplicitly] private GameObject _nestObject;
		private Collider2D _nestCollider;
		[SerializeField] [UsedImplicitly] private GameObject _sheepObject;
		private Collider2D _sheepCollider;
		private SheepController _sheepController;

		public ReactiveProperty<bool> IsChasingSheep
		{
			get { return _isChasingSheepProperty ?? (_isChasingSheepProperty = new ReactiveProperty<bool>(false)); }
		}

		public List<CrowController> CrowCollection
		{
			get { return _crowCollection ?? (_crowCollection = new List<CrowController>(_crowCount)); }	
		}

		public int CrowCount { get { return _crowCount; } }

		public float CrowLogicUpdateDelta
		{
			get { return _crowLogicUpdateDelta; }
		}

		public System.TimeSpan CrowLogicUpdateTimeSpan
		{
			get { return System.TimeSpan.FromSeconds(_crowLogicUpdateDelta); }
		}

		public GameObject CrowPrefab
		{
			get { return _crowPrefab; }
		}

		public Collider2D NestCollider
		{
			get { return _nestCollider ?? (_nestCollider = _nestObject.GetComponent<Collider2D>()); }
		}

		public GameObject NestObject
		{
			get { return _nestObject; }
		}

		public Vector2 NestPosition
		{
			get { return NestCollider.bounds.center; }
		}

		public Collider2D SheepCollider
		{
			get
			{
				return _sheepCollider 
				       ?? (_sheepCollider = _sheepObject != null 
					                      ? _sheepObject.GetComponent<Collider2D>() 
					                      : null); 
			}
		}

		public SheepController SheepController
		{
			get
			{
				return _sheepController 
				       ?? (_sheepController = _sheepObject != null 
				                            ? _sheepObject.GetComponent<SheepController>() 
											: null);
			}
		}


		public GameObject SheepObject
		{
			get { return _sheepObject; }
			set { _sheepObject = value; }
		}

		public Vector2 SheepPosition
		{
			get { return SheepCollider.bounds.center; }
		}
	}
}
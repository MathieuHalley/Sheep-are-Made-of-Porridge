using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class CrowTreeControllerData : ReactiveControllerData
	{
		[SerializeField] [UsedImplicitly] private int _crowCount;
		[SerializeField] [UsedImplicitly] private float _crowLogicUpdateDelta;
		[SerializeField] [UsedImplicitly] private GameObject _crowPrefab;
		[SerializeField] [UsedImplicitly] private GameObject _nestObject;
		[SerializeField] [UsedImplicitly] private GameObject _sheepObject;

		private ReactiveProperty<bool> _isChasingSheepProperty;
		private List<CrowController> _crowCollection;
		private Collider2D _nestCollider;
		private Collider2D _sheepCollider;
		private SheepController _sheepController;

		public ReactiveProperty<bool> IsChasingSheep
		{
			get
			{
				return GetBoolReactiveProperty(ref _isChasingSheepProperty, false);
			}
		}

		public List<CrowController> CrowCollection
		{
			get
			{
				if (_crowCollection != null) return _crowCollection;
				return _crowCollection = new List<CrowController>(_crowCount);
			}	
		}

		public int CrowCount
		{
			get { return _crowCount; }
		}

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
			get { return GetCollider2D(ref _nestCollider, _nestObject); }
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
			get { return GetCollider2D(ref _sheepCollider, _sheepObject); }
		}

		public SheepController SheepController
		{
			get { return GetReactiveController(ref _sheepController, _sheepObject); }
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
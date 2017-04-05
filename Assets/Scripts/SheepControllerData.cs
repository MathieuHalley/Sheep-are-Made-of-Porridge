using JetBrains.Annotations;
using UnityEngine;
using UniRx;

namespace Assets.Scripts
{
	[System.Serializable]
	public class SheepControllerData : ReactiveControllerData
	{
		[SerializeField] [UsedImplicitly] private ReactiveProperty<bool> _isGroundedProperty;
		[SerializeField] [UsedImplicitly] private ReactiveProperty<bool> _isJumpingProperty;
		[SerializeField] [UsedImplicitly] private ReactiveProperty<bool> _isPorridgeProperty;
		[SerializeField] [UsedImplicitly] private float _jumpHeight = 1f;
		[SerializeField] [UsedImplicitly] private float _groundCheckRadius = 0.1f;
		[SerializeField] [UsedImplicitly] private string _groundLayer = "Ground";
		[SerializeField] [UsedImplicitly] private MovementParameters _movementParameters;
		[SerializeField] [UsedImplicitly] private GameObject _porridgeBody;
		[SerializeField] [UsedImplicitly] private Collider2D _porridgeCollider;
		[SerializeField] [UsedImplicitly] private GameObject _sheepBody;
		[SerializeField] [UsedImplicitly] private Collider2D _sheepCollider;

		public ReactiveProperty<bool> IsGroundedProperty
		{
			get { return _isGroundedProperty ?? (_isGroundedProperty = new ReactiveProperty<bool>(false)); }
		}

		public ReactiveProperty<bool> IsJumpingProperty
		{
			get { return _isJumpingProperty ?? (_isJumpingProperty = new ReactiveProperty<bool>(false)); }
		}

		public ReactiveProperty<bool> IsPorridgeProperty
		{
			get { return _isPorridgeProperty ?? (_isPorridgeProperty = new ReactiveProperty<bool>(_porridgeCollider.enabled)); }
		}

		public float JumpHeight
		{
			get { return _jumpHeight; }
		}

		public float GroundCheckRadius
		{
			get { return _groundCheckRadius; }
		}

		public string GroundLayer
		{
			get { return _groundLayer; }
		}

		public MovementParameters MovementParameters
		{
			get { return _movementParameters; }
		}

		public GameObject PorridgeBody
		{
			get { return _porridgeBody; }
		}

		public Collider2D PorridgeCollider
		{
			get { return _porridgeCollider; }
		}

		public GameObject SheepBody
		{
			get { return _sheepBody; }
		}

		public Collider2D SheepCollider
		{
			get { return _sheepCollider; }
		}
	}
}
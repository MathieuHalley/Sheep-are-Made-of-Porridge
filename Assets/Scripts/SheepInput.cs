using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts
{
	public class SheepInput : MonoBehaviour
	{
		[SerializeField]
		private SheepController _sheepController;
		private Rigidbody2D _rigidbody;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_sheepController = GetComponent<SheepController>();
		}

		private void Start()
		{
			MovementInputSubscription();
			JumpInputSubscription();
			SetPorridgeModeInputSubscription();
			SetSheepModeInputSubscription();
		}

		private void MovementInputSubscription()
		{
			this.FixedUpdateAsObservable()
				.Select(_ => new Vector2(Input.GetAxis("Horizontal"), 0))
				.Where(v => Math.Abs(v.x) > 0.05f || Mathf.Abs(_rigidbody.velocity.x) > 0.05f)
				.Subscribe(v => _sheepController.ProcessMovementInput(v))
				.AddTo(this);
		}

		private void JumpInputSubscription()
		{
			this.FixedUpdateAsObservable()
				.Where(_ => Input.GetKey(KeyCode.Space))
				.Subscribe(_ => _sheepController.ProcessJumpInput())
				.AddTo(this);
		}

		private void SetPorridgeModeInputSubscription()
		{
			this.UpdateAsObservable()
				.Where(_ => Input.GetAxis("Vertical") < 0)
				.Subscribe(_ => _sheepController.IsPorridge = true)
				.AddTo(this);
		}

		private void SetSheepModeInputSubscription()
		{
			this.UpdateAsObservable()
				.Where(_ => Input.GetAxis("Vertical") > 0)
				.Subscribe(_ => _sheepController.IsPorridge = false)
				.AddTo(this);
		}
	}
}
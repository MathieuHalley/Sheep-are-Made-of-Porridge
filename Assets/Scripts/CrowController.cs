using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class CrowController : MonoBehaviour
{
	private Rigidbody2D _rigidbody;
	[SerializeField]
	private CrowControllerData _data;
	private CrowControllerData Data { get { return _data; } }
	private Vector2 CurPosition { get { return (Vector2)this.gameObject.transform.position; } }

	public float VelocityMagnitude;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		MoveTowardsNestSubscription();
		MoveTowardsTargetSubscription();
	}

	public void SetNest(Vector2 nest)
	{
		Data.Nest = nest;
	}

	public void SetTarget(Vector2 target)
	{
		Data.Target = target;
	}

	private IDisposable MoveTowardsNestSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.Target != CurPosition && Data.Target == Data.Nest)
			.Select(_ => Data.Target)
			.Subscribe(_ => MoveTowards(Data.Nest))
			.AddTo(this);
	}

	private IDisposable MoveTowardsTargetSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.Target != CurPosition && Data.Target != Data.Nest)
			.Select(_ => Data.Target)
			.Subscribe(_ => MoveTowards(Data.Target))
			.AddTo(this);
	}

	private void MoveTowards(Vector2 target)
	{
		MovementParameters movementParams = Data.MovementParameters;
		float maxVelocity = movementParams.MaxVelocity;
		Vector2 movementDistance = target - CurPosition;
		movementDistance = Vector2.ClampMagnitude(movementDistance, maxVelocity * Time.fixedDeltaTime);
		_rigidbody.MovePosition(movementDistance + CurPosition);
	}
}

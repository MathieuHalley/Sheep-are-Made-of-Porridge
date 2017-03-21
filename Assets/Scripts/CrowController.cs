using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class CrowController : ReactiveController<CrowControllerData>
{
	private void Start()
	{
		BuildFlightPathToNestSubscription();
		BuildFlightPathToTargetSubscription();
		FollowFlightCurveSubscription();
		ReturnToNestTriggerSubscription();
	}

	public void SetNest(Vector2 nest)
	{
		Data.NestPosition = nest;
	}

	public void SetTargetPosition(Vector2 target)
	{
		Data.TargetPosition = target;
		Data.IsActive = true;
	}

	public void ReturnToNest()
	{
		Data.IsActive = false;
	}

	private System.IDisposable ReturnToNestTriggerSubscription()
	{
		return
		this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player" || (Vector2)CurPosition == Data.TargetPosition)
			.Subscribe(_ => Data.IsActive = false)
			.AddTo(this);
	}
	private System.IDisposable BuildFlightPathToNestSubscription()
	{
		return
		Data.IsActiveProperty.AsObservable()
			.DistinctUntilChanged()
			.Where(_ => !Data.IsActive)
			.Do(_ => Debug.Log("Build flightpath to nest!"))
			.Subscribe(_ => Data.Flight = new FlightCurve(Data.HomewardFlight, CurPosition, Data.NestPosition))
			.AddTo(this);
	}

	private System.IDisposable BuildFlightPathToTargetSubscription()
	{
		return
		Data.TargetPositionProperty.AsObservable()
			.DistinctUntilChanged()
			.Where(_ => Data.IsActive && (Vector2)CurPosition != Data.TargetPosition)
			.Do(_ => Debug.Log("Build flightpath to target!"))
			.Subscribe(_ => Data.Flight = new FlightCurve(Data.OutwardFlight, CurPosition, Data.TargetPosition))
			.AddTo(this);
	}

	private System.IDisposable FollowFlightCurveSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.IsActive || (Vector2)CurPosition != Data.NestPosition )
			.Subscribe(_ =>
			{
				Vector2 direction = (Data.Flight.End - CurPosition).normalized;
				float maxDistance = Data.MovementParameters.MaxVelocity * Time.fixedDeltaTime;
				Vector2 newPosition = Data.Flight.Evaluate(CurPosition + direction * maxDistance);
				Rigidbody.MovePosition(newPosition);
//				Rigidbody.AddForce(newPosition - CurPosition, ForceMode2D.Impulse);
//				Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, Data.MovementParameters.MaxVelocity);
				Debug.DrawRay(CurPosition, direction);
				Debug.DrawLine(CurPosition, newPosition);
				Debug.DrawLine(Data.Flight.Start, Data.Flight.End);
				Data.Flight.DrawCurve();
			})
			.AddTo(this);
	}
}

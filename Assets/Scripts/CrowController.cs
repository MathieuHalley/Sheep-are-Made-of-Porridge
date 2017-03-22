using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class CrowController : ReactiveController<CrowControllerData>
{
	private void Start()
	{
		ReturnToNestTriggerSubscription();
		FollowFlightCurveSubscription();
	}

	public CrowController SetNestPosition(Vector2 nest)
	{
		Data.NestPosition = nest;
		return this;
	}

	public CrowController DefineSheep(Transform sheep)
	{
		Data.Sheep = sheep;
		return this;
	}

	public CrowController FlyToNest()
	{
		if (Data.Flight.End != Data.NestPosition)
		{
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 1);
			Data.Flight = new FlightCurve(Data.HomewardFlight, CurPosition, Data.NestPosition);
		}
		return this;
	}

	public CrowController FlyToSheep()
	{
		Debug.DrawLine(Data.SheepBounds.min, new Vector2(Data.SheepBounds.min.x, Data.SheepBounds.max.y), Color.yellow);
		Debug.DrawLine(Data.SheepBounds.min, new Vector2(Data.SheepBounds.max.x, Data.SheepBounds.min.y), Color.yellow);
		Debug.DrawLine(Data.SheepBounds.max, new Vector2(Data.SheepBounds.min.x, Data.SheepBounds.max.y), Color.yellow);
		Debug.DrawLine(Data.SheepBounds.max, new Vector2(Data.SheepBounds.max.x, Data.SheepBounds.min.y), Color.yellow);
		if (Data.Flight.End != (Vector2)Data.Sheep.position && !Data.SheepBounds.Contains(CurPosition))
		{
//			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 1);
			Data.Flight = new FlightCurve(Data.OutwardFlight, CurPosition, Data.Sheep.position);
		}
		return this;
	}

	private System.IDisposable ReturnToNestTriggerSubscription()
	{
		return
		this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player" || ((Vector2)CurPosition == Data.Flight.End && Data.Flight.End != Data.NestPosition))
			.Subscribe(_ => FlyToNest())
			.AddTo(this);
	}

	private System.IDisposable FollowFlightCurveSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => (Vector2)CurPosition != Data.Flight.End )
			.Subscribe(_ =>
			{
				Vector2 direction = (Data.Flight.End - CurPosition).normalized;
				float maxDistance = Data.MovementParameters.MaxVelocity * Time.fixedDeltaTime;
				Vector2 newPosition = Data.Flight.Evaluate(CurPosition + direction * maxDistance);
//				Rigidbody.MovePosition(newPosition);
				Rigidbody.AddForce(newPosition - CurPosition, ForceMode2D.Impulse);
				Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, Data.MovementParameters.MaxVelocity);
				Debug.DrawRay(CurPosition, direction);
				Debug.DrawLine(CurPosition, newPosition);
				Debug.DrawLine(Data.Flight.Start, Data.Flight.End);
				Data.Flight.DrawCurve();
			})
			.AddTo(this);
	}
}

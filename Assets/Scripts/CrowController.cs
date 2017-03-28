using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class CrowController : ReactiveController<CrowControllerData>
{
	public ReactiveProperty<bool> IsFlightActiveProperty { get; private set; }
	public bool IsFlightActive
	{
		get { return IsFlightActiveProperty.Value; }
		set { IsFlightActiveProperty.Value = value; }
	}
	public float CurrentFlightProgress
	{
		get
		{
			return (Data.FlightSchedule.Count > 0) 
				? Data.CurrentFlight.Progress(CurPosition) 
				: (CurPosition == Data.FlightTarget.target ? 1f : 0f);
		}
	}
	public float CurrentFlightLength
	{
		get
		{
			return Data.FlightSchedule.Count > 0 
				? Data.CurrentFlight.Length 
				: (CurPosition - Data.FlightTarget.target).magnitude; }
	}

	private void Awake()
	{
		IsFlightActiveProperty = new ReactiveProperty<bool>(false);
		Data.FlightSchedule = new Queue<FlightPath>();
		Data.FlightTarget = this.gameObject.GetComponent<TargetJoint2D>();
		if (Data.FlightTarget == null)
		{
			Data.FlightTarget = this.gameObject.AddComponent<TargetJoint2D>();
		}
		Data.FlightTarget.maxForce = Data.MaxVelocity;
	}

	private void Start()
	{
		FollowFlightPathSubscription();
	}
	
	public void EnqueueFlightPath(Vector2 start, Vector2 target)
	{
		Data.FlightSchedule.Enqueue(new FlightPath(start, target));
		Data.FlightTarget.target = Data.CurrentFlight.Target;
		SetIsFlightActive(true);
	}

	public void SetFlightPath(Vector2 start, Vector2 target)
	{
		Data.FlightSchedule.Clear();
		Data.FlightSchedule.Enqueue(new FlightPath(start, target));
		Data.FlightTarget.target = Data.CurrentFlight.Target;
		SetIsFlightActive(true);
	}

	public void ClearFlights()
	{
		Data.FlightSchedule.Clear();
		SetIsFlightActive(false);
	}

	private void SetIsFlightActive(bool isFlightActive)
	{
		IsFlightActive = isFlightActive;
		Data.FlightTarget.enabled = isFlightActive;
	}

	private System.IDisposable FollowFlightPathSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.FlightSchedule.Count > 0)
			.Subscribe(_ =>
			{
				foreach(FlightPath flight in Data.FlightSchedule)
				{
					flight.DrawFlightPath();
				}

				if (Data.CurrentFlight.Progress(CurPosition) > 0.99f || CurrentFlightLength < 0.01f)
				{
					Data.FlightSchedule.Dequeue();
					if (Data.FlightSchedule.Count == 0)
					{
						SetIsFlightActive(false);
					}
					else
					{
						Data.FlightTarget.target = Data.CurrentFlight.Target;
					}
				}
				else
				{
					float pathOffset = Data.CurrentFlight.Evaluate(CurPosition).y - CurPosition.y;
					Rigidbody.AddForce(Vector2.up * pathOffset, ForceMode2D.Force);
					Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, Data.MaxVelocity);
				}
			})
			.AddTo(this);
	}

}

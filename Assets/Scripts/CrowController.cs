using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts
{
	public class CrowController : ReactiveController<CrowControllerData>
	{
		public FlightPath CurrentFlight { get { return Data.CurrentFlight;} }

		private void Start()
		{
			Data.FlightTarget = GetComponent<TargetJoint2D>();
			FollowFlightPathSubscription();
			OnSheepIsPorridgeSubscription();
		}

		public bool CalcuateIsCurrentFlightActive()
		{
			var flightDistanceRemaining = (Data.FlightSchedule.Count > 0)
				? Data.CurrentFlight.GetFlightDistanceRemaining(CurPosition)
				: (CurPosition - Data.FlightTarget.target).magnitude;
			var speed = Data.MaxVelocity * Time.fixedDeltaTime;
			var isCurrentFlightActive = flightDistanceRemaining < speed;
			Data.IsCurrentFlightActiveProperty.Value = isCurrentFlightActive;
			return isCurrentFlightActive;
		}

		public bool GetIsCurrentFlightActive()
		{
			return Data.IsCurrentFlightActiveProperty.Value;
		}

		public void SetIsCurrentFlightActive(bool isActive)
		{
			Data.FlightTarget.enabled = isActive;
			Data.IsCurrentFlightActiveProperty.Value = isActive;
		}

		public void ClearFlights()
		{
			Data.FlightSchedule.Clear();
			SetIsCurrentFlightActive(false);
		}

		public void EnqueueFlightPath(
			Vector2 start,
			Vector2 target,
			float outTangentAngleStart = 0f,
			float inTangentAngleTarget = 0f)
		{
			if (start == target) return;
			Data.FlightSchedule.Enqueue(new FlightPath(start, target, outTangentAngleStart, inTangentAngleTarget));
			Data.FlightTarget.target = Data.CurrentFlight.Target;
			SetIsCurrentFlightActive(true);
		}

		public void SetFlightPath(
			Vector2 start,
			Vector2 target,
			float outTangentAngleStart = 0f,
			float inTangentAngleTarget = 0f)
		{
			Data.FlightSchedule.Clear();
			EnqueueFlightPath(start, target, outTangentAngleStart, inTangentAngleTarget);
		}

		private void FollowFlightPathSubscription()
		{
			this.FixedUpdateAsObservable()
				.Where(_ => Data.FlightSchedule.Count > 0 && Data.IsCurrentFlightActiveProperty.Value)
				.Subscribe(_ =>
				{
					foreach (var flight in Data.FlightSchedule) flight.DrawFlightPath();
					var vectorToPath = (Data.CurrentFlight.Evaluate(CurPosition) - CurPosition);
					Rigidbody.AddForce(vectorToPath, ForceMode2D.Impulse);
					Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, Data.MaxVelocity);
				})
				.AddTo(this);
		}

		private void OnSheepIsPorridgeSubscription()
		{
			Data.IsCurrentFlightActiveProperty
				.AsObservable()
				.DistinctUntilChanged()
				.Subscribe(isCurrentFlightActive =>
				{
					if (isCurrentFlightActive || Data.FlightSchedule.Count == 0) return;
					Data.FlightSchedule.Dequeue();
					if (Data.FlightSchedule.Count == 0)
						SetIsCurrentFlightActive(false);
					else
						Data.FlightTarget.target = Data.CurrentFlight.Target;
				})
				.AddTo(this);
		}
	}
}
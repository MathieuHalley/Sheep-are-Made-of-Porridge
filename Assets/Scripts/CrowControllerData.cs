using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class CrowControllerData : ReactiveControllerData
	{
		private TargetJoint2D _flightTarget;
		private Queue<FlightPath> _flightSchedule;
		private ReactiveProperty<bool> _isCurrentFlightActiveProperty;
		[SerializeField] [UsedImplicitly] private float _maxVelocity;

		public FlightPath CurrentFlight
		{
			get
			{
				return FlightSchedule.Count > 0 ? FlightSchedule.Peek() : null;
			}
		}

		public Queue<FlightPath> FlightSchedule
		{
			get
			{
				return _flightSchedule ?? (_flightSchedule = new Queue<FlightPath>());
			}
		}

		public TargetJoint2D FlightTarget
		{
			get { return _flightTarget; }
			set
			{
				_flightTarget = value;
				_flightTarget.maxForce = _maxVelocity;
			}
		}

		public ReactiveProperty<bool> IsCurrentFlightActiveProperty
		{
			get { return GetBoolReactiveProperty(ref _isCurrentFlightActiveProperty, false); }
		}

		public float MaxVelocity
		{
			get { return _maxVelocity; }
		}
	}
}
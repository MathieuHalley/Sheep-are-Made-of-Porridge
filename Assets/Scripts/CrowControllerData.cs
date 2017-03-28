using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CrowControllerData : ReactiveControllerData
{
	[SerializeField]
	private float _maxVelocity;
	public float MaxVelocity { get { return _maxVelocity; } }
	public FlightPath CurrentFlight { get { return FlightSchedule.Peek(); } }
	public Queue<FlightPath> FlightSchedule { get; set; }
	public TargetJoint2D FlightTarget { get; set; }
}

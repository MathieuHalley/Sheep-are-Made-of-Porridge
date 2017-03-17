using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class CrowController : ReactiveController<CrowControllerData>
{
	private void Start()
	{
//		MoveTowardsNestSubscription();
//		MoveTowardsTargetSubscription();
//		DeactivateAtTargetSubscription();
	}

	public void SetNest(Vector2 nest)
	{
		Data.NestPosition = nest;
	}

	public void SetTarget(Vector2 target)
	{
		Data.Target = target;
		Data.IsActive = true;
		Data.FlightPath = new List<Vector3>();
		Data.FlightPath.AddRange(BuildFlightPath(Data.OutwardFlightCurve, Data.NestPosition, target));
		Data.FlightPath.AddRange(BuildFlightPath(Data.HomewardFlightCurve, target, Data.NestPosition));
		DrawFlightPath(Data.FlightPath, Data.NestPosition, target);
	}

	public void ReturnToNest()
	{
		Data.IsActive = false;
	}

	private System.IDisposable MoveTowardsNestSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => !Data.IsActive)
			.Select(_ => Data.NestPosition)
			.Subscribe(nest => MoveTowards(Data.HomewardFlightCurve, CurPosition, Data.NestPosition))
			.AddTo(this);
	}

	private System.IDisposable MoveTowardsTargetSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.IsActive)
			.Select(_ => Data.Target)
			.Subscribe(target => MoveTowards(Data.OutwardFlightCurve, Data.NestPosition, target))
			.AddTo(this);
	}

	private System.IDisposable DeactivateAtTargetSubscription()
	{
		return
		Data.IsActiveProperty.AsObservable()
			.Where(isActive => isActive && Data.Target == CurPosition)
			.Subscribe(_ => Data.IsActive = false)
			.AddTo(this);
	}

	private void MoveTowards(AnimationCurve flightPath, Vector2 pathStart, Vector2 pathEnd)
	{	

	}

	private List<Vector3> BuildFlightPath(AnimationCurve flightPath, Vector2 pathStart, Vector2 pathEnd)
	{
		List<Vector3> tempFlightPath = new List<Vector3>();
		Vector2 pathEndOffset = pathEnd - pathStart;
		Vector2 pathEndDirection = new Vector2(Mathf.Sign(pathEndOffset.x), Mathf.Sign(pathEndOffset.y));
		float minPathElevation = Mathf.Min(pathEnd.y, pathStart.y);
		foreach (Keyframe key in flightPath.keys)
		{
			tempFlightPath.Add(
				new Vector2(
					pathStart.x + key.time * pathEndOffset.x,
					(pathEndDirection.y == -1f)
						? Mathf.Min(pathEnd.y, pathStart.y) + key.value * -pathEndOffset.y
						: Mathf.Max(pathEnd.y, pathStart.y) - key.value * pathEndOffset.y));
		}

		for (int i = 1; i < tempFlightPath.Count; ++i)
		{
			int h = i - 1;
			float distance = Vector2.Distance(tempFlightPath[i], tempFlightPath[h]);
			if (Vector2.Distance(tempFlightPath[i], tempFlightPath[h]) == 0)
			{
				tempFlightPath.RemoveAt(i);
				--i;
				continue;
			}
			if (Vector2.Distance(tempFlightPath[i], tempFlightPath[h]) > Data.MovementParameters.MaxVelocity * Time.fixedDeltaTime)
			{
				int numberOfMidpoints = Mathf.CeilToInt(distance / (Data.MovementParameters.MaxVelocity * Time.fixedDeltaTime * 15));
				float distanceBetweenMidpoints = distance / numberOfMidpoints;
				Vector3[] midpoints = new Vector3[numberOfMidpoints];
				for (int j = 0; j < numberOfMidpoints; ++j)
				{
					midpoints[j] = 
						Vector2.MoveTowards(
							tempFlightPath[h], 
							tempFlightPath[i], 
							distanceBetweenMidpoints);
					float distanceAlongPath = Mathf.InverseLerp(pathStart.x, pathEnd.x, midpoints[j].x);
					midpoints[j].y =
						(pathEndDirection.y == -1f)
						? Mathf.Min(pathEnd.y, pathStart.y) + flightPath.Evaluate(distanceAlongPath) * -pathEndOffset.y
						: Mathf.Max(pathEnd.y, pathStart.y) - flightPath.Evaluate(distanceAlongPath) * pathEndOffset.y;
					tempFlightPath.Insert(i, midpoints[j]);
					i++;
					h = i - 1;
				}
			}
		}
		return new List<Vector3>(tempFlightPath);
	}
	private void DrawFlightPath(List<Vector3> flightPath, Vector2 pathStart, Vector2 pathEnd)
	{
		string pathCoordinates = "";
		Vector2 pathEndOffset = pathEnd - pathStart;
		Vector2 pathEndDirection = new Vector2(Mathf.Sign(pathEndOffset.x), Mathf.Sign(pathEndOffset.y));
		for (int i = 1; i < Data.FlightPath.Count; ++i)
		{
			Debug.DrawLine(Data.FlightPath[i - 1], Data.FlightPath[i], Color.red, 1f);
			Debug.DrawLine(Data.FlightPath[i - 1] + Vector3.up * 0.05f, Data.FlightPath[i - 1] + Vector3.down * 0.05f, Color.green, 1f);
			Debug.DrawLine(Data.FlightPath[i - 1] + Vector3.left * 0.05f, Data.FlightPath[i - 1] + Vector3.right * 0.05f, Color.green, 1f);
			pathCoordinates += (Vector2)Data.FlightPath[i - 1] + ", ";
		}
//		Debug.Log("Path Coordinates: " + pathCoordinates);
//		Debug.Log("Path End direction: " + pathEndDirection);
	}
}

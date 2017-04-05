using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class FlightPath
	{
		public AnimationCurve Path { get; private set; }
		public Vector2 Start { get; private set; }
		public Vector2 Target { get; private set; }
		public float FlightLength { get; private set; }
		public Rect PathZone { get; private set; }

		public FlightPath(
			Vector2 start,
			Vector2 target,
			float outTangentAngleStart = 0f,
			float inTangentAngleTarget = 0f)
		{
			Start = start;
			Target = target;
			Path = CreateFlightPath(start, target, outTangentAngleStart, inTangentAngleTarget);
			FlightLength = (target - start).magnitude;
			PathZone = new Rect(
				new Vector2(
					Mathf.Min(start.x, target.x),
					Mathf.Min(start.y, target.y)),
				new Vector2(
					Mathf.Abs(start.x - target.x),
					Mathf.Abs(start.y - target.y)));
		}

		public void DrawFlightPath()
		{
			var currNode = new Vector2();
			var prevNode = Evaluate(new Vector2(Start.x, 0));
			var time = 0f;
			while (time <= 1f)
			{
				time += 0.1f;
				var currNodeTime = Mathf.Lerp(Start.x, Target.x, time);
				if (currNode != default(Vector2)) prevNode = currNode;
				currNode = Evaluate(new Vector2(currNodeTime, 0));

				Debug.DrawLine(prevNode, currNode, Color.Lerp(Color.yellow, Color.red, time), 1f);
				Debug.DrawLine(currNode + Vector2.up * 0.05f, currNode + Vector2.down * 0.05f, Color.green, 0.25f);
				Debug.DrawLine(currNode + Vector2.left * 0.05f, currNode + Vector2.right * 0.05f, Color.green, 0.25f);
			}
			Debug.DrawLine(PathZone.min, new Vector2(PathZone.xMin, PathZone.yMax), Color.gray);
			Debug.DrawLine(PathZone.min, new Vector2(PathZone.xMax, PathZone.yMin), Color.gray);
			Debug.DrawLine(PathZone.max, new Vector2(PathZone.xMin, PathZone.yMax), Color.gray);
			Debug.DrawLine(PathZone.max, new Vector2(PathZone.xMax, PathZone.yMin), Color.gray);
		}

		public Vector2 Evaluate(Vector2 position)
		{
			var progress = GetFlightPercentProgress(position);
			if (Mathf.Approximately(progress, 0)) return Start;
			if (Mathf.Approximately(progress, 1)) return Target;
			var positionHeight = Path.Evaluate(progress);
			return new Vector2(position.x, positionHeight);
		}

		public float GetFlightPercentProgress(Vector2 position)
		{
			return Mathf.InverseLerp(Start.x, Target.x, position.x);
		}

		public float GetFlightDistanceTravelled(Vector2 position)
		{
			return (position - Start).magnitude;
		}

		public float GetFlightDistanceRemaining(Vector2 position)
		{
			return (position - Target).magnitude;
		}

		public Vector2 GetDirectionToStart(Vector2 position)
		{
			return (Start - position).normalized;
		}

		public Vector2 GetDirectionToTarget(Vector2 position)
		{
			return (Target - position).normalized;
		}

		private static AnimationCurve CreateFlightPath(
			Vector2 start,
			Vector2 target,
			float outTangentAngleStart = 0f,
			float inTangentAngleTarget = 0f)
		{
			var baseFlightPath = new AnimationCurve(
				new Keyframe(0, start.y, 0, Mathf.Deg2Rad * outTangentAngleStart),
				new Keyframe(1, target.y, Mathf.Deg2Rad * inTangentAngleTarget, 0));
			return baseFlightPath;
		}
	}
}
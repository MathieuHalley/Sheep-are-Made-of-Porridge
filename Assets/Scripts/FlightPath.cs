using UnityEditor;
using UnityEngine;

[System.Serializable]
public class FlightPath
{
	public AnimationCurve Path { get; private set; }
	public Vector2 Start { get; private set; }
	public Vector2 Target { get; private set; }
	public float Length { get; private set; }
	private Rect _pathZone = new Rect();

	public FlightPath(Vector2 start, Vector2 target)
	{
		Start = start;
		Target = target;
		Path = CreateFlightPath(start, target);
		Length = (target - start).magnitude;
		_pathZone.min = new Vector2(Mathf.Min(start.x, target.x), Mathf.Min(start.y, target.y));
		_pathZone.max = new Vector2(Mathf.Max(start.x, target.x), Mathf.Max(start.y, target.y));
	}
	
	public Vector3 Evaluate(Vector3 position)
	{
		float progress = Mathf.InverseLerp(Start.x, Target.x, position.x);
		if (Mathf.Approximately(progress, 0))
			return Start;
		if (Mathf.Approximately(progress, 1))
			return Target;
		float positionHeight = Path.Evaluate(progress);
		return new Vector2(position.x, positionHeight);
	}

	public void DrawFlightPath()
	{
		float currNodeTime;
		Vector2 currNode = new Vector3();
		Vector2 prevNode = Evaluate(new Vector2(Start.x, 0));
		float time = 0.0f;
		while(time <= 1f)
		{
			time += 0.1f;
			currNodeTime = Mathf.Lerp(Start.x, Target.x, time);
			if (currNode != default(Vector2))
				prevNode = currNode;
			currNode = Evaluate(new Vector2(currNodeTime, 0));
			
			Debug.DrawLine(prevNode, currNode, Color.Lerp(Color.yellow, Color.red, time), 1f);
			Debug.DrawLine(currNode + Vector2.up * 0.05f, currNode + Vector2.down * 0.05f, Color.green, 0.25f); 
			Debug.DrawLine(currNode + Vector2.left * 0.05f, currNode + Vector2.right * 0.05f, Color.green, 0.25f);
		}
		Debug.DrawLine(_pathZone.min, new Vector2(_pathZone.min.x, _pathZone.max.y), Color.gray);
		Debug.DrawLine(_pathZone.min, new Vector2(_pathZone.max.x, _pathZone.min.y), Color.gray);
		Debug.DrawLine(_pathZone.max, new Vector2(_pathZone.min.x, _pathZone.max.y), Color.gray);
		Debug.DrawLine(_pathZone.max, new Vector2(_pathZone.max.x, _pathZone.min.y), Color.gray);
	}

	public float Progress(Vector2 curPosition)
	{
		float startOffset = (curPosition - Start).magnitude;
		return startOffset / Length;
	}

	private AnimationCurve CreateFlightPath(Vector3 start, Vector3 target)
	{
		Keyframe startFrame = new Keyframe(0, start.y);
		Keyframe targetFrame = new Keyframe(1, target.y);
		AnimationCurve path = new AnimationCurve(startFrame, targetFrame);
		return path;
	}
}


using UnityEditor;
using UnityEngine;

[System.Serializable]
public class FlightCurve
{
	[SerializeField]
	private AnimationCurve _curve;
	[SerializeField]
	private Vector2 _start = new Vector2();
	[SerializeField]
	private Vector2 _end = new Vector2();

	public AnimationCurve Curve { get { return _curve; } }
	public Vector2 Start { get { return _start; } }
	public Vector2 End { get { return _end; } }

	private Bounds _bounds = new Bounds();
	private AnimationCurve _directCurve;

	public FlightCurve(AnimationCurve curve, Vector2 start, Vector2 end)
	{
		_curve = new AnimationCurve();
		_start = start;
		_end = end;
		SetBounds();
		_directCurve = new AnimationCurve(new Keyframe(0, _start.y), new Keyframe(1, _end.y));
		for (int i = 0; i < _directCurve.length; i++)
		{
			AnimationUtility.SetKeyLeftTangentMode(_directCurve, i, AnimationUtility.TangentMode.Linear);
			AnimationUtility.SetKeyRightTangentMode(_directCurve, i, AnimationUtility.TangentMode.Linear);
		}
		for (int i = 0; i < curve.length; i++)
		{
			_curve.AddKey(new Keyframe(curve[i].time, curve.Evaluate(curve[i].time) + _directCurve.Evaluate(curve[i].time)));
		}
	}

	public Vector2 Evaluate(Vector2 position)
	{
		float progress = Mathf.InverseLerp(_start.x, _end.x, position.x);
		if (Mathf.Approximately(progress, 0))
			return _start;
		if (Mathf.Approximately(progress, 1))
			return _end;
		//		float positionHeight = _curve.Evaluate(progress) + _directCurve.Evaluate(progress);
		float positionHeight = _curve.Evaluate(progress);
		return new Vector2(position.x, positionHeight);
	}

	public void DrawCurve()
	{
		Vector3 currNode = new Vector3();
		Vector3 prevNode = new Vector3();
		for (float t = 0.1f; t <= 1f; t += 0.1f)
		{
			prevNode = currNode == default(Vector3)
				? (Vector3)Evaluate(new Vector2(Mathf.Lerp(_start.x, _end.x, t - 0.1f), 0))
				: currNode;
			currNode = Evaluate(new Vector2(Mathf.Lerp(_start.x, _end.x, t), 0));

			Debug.DrawLine(prevNode, currNode, Color.red, 1f);
			Debug.DrawLine(
				currNode + Vector3.up * 0.05f,
				currNode + Vector3.down * 0.05f,
				Color.green, 1f);
			Debug.DrawLine(
				currNode + Vector3.left * 0.05f,
				currNode + Vector3.right * 0.05f,
				Color.green, 1f);
		}
		Debug.DrawLine(_bounds.min, new Vector2(_bounds.min.x, _bounds.max.y), Color.gray);
		Debug.DrawLine(_bounds.min, new Vector2(_bounds.max.x, _bounds.min.y), Color.gray);
		Debug.DrawLine(_bounds.max, new Vector2(_bounds.min.x, _bounds.max.y), Color.gray);
		Debug.DrawLine(_bounds.max, new Vector2(_bounds.max.x, _bounds.min.y), Color.gray);
	}

	private void SetBounds()
	{
		_bounds.SetMinMax(
			new Vector2(Mathf.Min(_start.x, _end.x), Mathf.Min(_start.y, _end.y)),
			new Vector2(Mathf.Max(_start.x, _end.x), Mathf.Max(_start.y, _end.y)));
	}
}


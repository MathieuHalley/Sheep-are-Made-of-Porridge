using UnityEngine;
using System.Collections;

public class CursorTracking : MonoBehaviour 
{
	public Vector2 cursorTrackDampeningMultiplier = new Vector2(1.0f, 1.0f);
	public float maxTrackDist;
	public float maxOffsetDistance;
	public Vector2 maxOffsetDistanceMultiplier = new Vector2(1.0f, 1.0f);

	private Vector2 cursorPos;
	private Vector2 originPos;
	void Awake()
	{
		Vector2 distToScreenEdge = new Vector2();
		originPos = this.transform.localPosition;
		if ( maxTrackDist == 0f )
		{
			distToScreenEdge = RelativeScreenToWorldPoint(Screen.width, Screen.height);
			maxTrackDist = distToScreenEdge.y * 0.5f;
		}

		if ( maxOffsetDistance == 0f )
		{
			maxOffsetDistance = maxTrackDist * 0.1f;
			maxOffsetDistanceMultiplier.y = distToScreenEdge.x / distToScreenEdge.y;
		}
	}

	void Update()
	{
		cursorPos = RelativeCursorPos();
		float percentCursorDistanceFromParent = Mathf.Min(1f, cursorPos.magnitude / maxOffsetDistance);
		Vector2 newObjectPos = cursorPos.normalized;

		newObjectPos.x *= cursorTrackDampeningMultiplier.x;
		newObjectPos.y *= cursorTrackDampeningMultiplier.y;

		newObjectPos *= maxOffsetDistance * percentCursorDistanceFromParent;
		newObjectPos.x *= maxOffsetDistanceMultiplier.x;
		newObjectPos.y *= maxOffsetDistanceMultiplier.y;

		this.transform.localPosition = originPos + newObjectPos;

		//	Calculate the perc distance between the cursor and its max track distance
		//	Set the local position of this object to be that perc distance to its parent

	}

	private Vector2 RelativeCursorPos()
	{
		return RelativeScreenToWorldPoint(Input.mousePosition.x, Input.mousePosition.y);
	}

	private Vector2 RelativeScreenToWorldPoint(float screenPosX, float screenPosY)
	{
		Vector2
		relativeScreenPos =
		Camera.main.ScreenToWorldPoint(
			new Vector3(screenPosX, screenPosY, this.transform.parent.position.z - Camera.main.transform.position.z))
		- this.transform.parent.position;

		return relativeScreenPos;
	}

}

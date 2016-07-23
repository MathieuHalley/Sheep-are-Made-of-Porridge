using UnityEngine;
using System.Collections;

public class FormNode : MonoBehaviour
{
	private float offset;
	private Rigidbody2D cachedRigidbody2D;

	public float Offset { get { return offset; } }
	public float Rotation
	{
		get { return cachedRigidbody2D.rotation; }
		set { cachedRigidbody2D.rotation = value; }
	}
	public Vector3 Position
	{
		get { return this.transform.position; }
		set { this.transform.position = value; }
	}

	public void Start()
	{
		this.offset = Mathf.Clamp01(offset);
		cachedRigidbody2D = this.GetComponent<Rigidbody2D>();
	}

	public void SetOffset(float offset)
	{
		this.offset = Mathf.Repeat(offset, 1f);
		this.name = this.offset.ToString();
	}

	public Vector3 CalcNodePosition(
		float xOffset, float yOffset,
		float xRadius, float yRadius, float angle, Space relativeTo = Space.World)
	{
		Vector3 newNodePos
			= new Vector3(
			xOffset + Mathf.Cos(angle) * xRadius,
			yOffset + Mathf.Sin(angle) * yRadius);

		if (relativeTo == Space.World)
			newNodePos += transform.parent.position;

		Position = newNodePos;
		return newNodePos;
	}

	public Vector3 UpdateNodePosition()
	{
		return new Vector3();
	}
}

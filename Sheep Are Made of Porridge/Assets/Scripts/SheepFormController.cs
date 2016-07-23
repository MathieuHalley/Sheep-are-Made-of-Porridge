using UnityEngine;
using System.Collections;
using System;

public class SheepFormController : FormController
{
	public float groundYOffset;
	public float bellyLength;
	public float leftXRadius;
	public float leftYRadius;
	public float rightXRadius;
	public float rightYRadius;

	public Vector3 LegLeft
	{
		get { return formVertices[0]; }
		set { formVertices[0] = value; }
	}
	public Vector3 BodyLeft
	{
		get { return formVertices[1]; }
		set { formVertices[1] = value; }
	}
	public Vector3 BodyTop
	{
		get { return formVertices[2]; }
		set { formVertices[2] = value; }
	}
	public Vector3 BodyRight
	{
		get { return formVertices[3]; }
		set { formVertices[3] = value; }
	}
	public Vector3 LegRight
	{
		get { return formVertices[4]; }
		set { formVertices[4] = value; }
	}

	protected override void UpdateVertexPositions(float offsetDir)
	{
	}

	protected void OnGUI()
	{
		GUI.Label(
			new Rect(0, 60, 500, 25),
			"BodyRight.x " + (BodyRight.x + sheepPosition.x).ToString());
		GUI.Label(
			new Rect(0, 85, 500, 25), 
			"LegRight.x " + (LegRight.x + sheepPosition.x).ToString());
	}

	protected override void PositionFormVertices()
	{
		LegLeft
			= new Vector3(-bellyLength * 0.5f, groundYOffset);
		LegRight
			= new Vector3(bellyLength * 0.5f, groundYOffset);
		BodyLeft
			= new Vector3(LegLeft.x - leftXRadius, groundYOffset + leftYRadius);
		BodyTop
			= new Vector3(0f, groundYOffset + (rightYRadius * 2f + leftYRadius * 2f) * 0.5f);
		BodyRight
			= new Vector3(LegRight.x + rightXRadius, groundYOffset + rightYRadius);
	}

	protected override Vector3 CalcNodePosition(FormNode node)
	{
		float relativePosToVert = (node.Offset % 0.2f) * 5;
		Vector3 newNodePos = Vector3.zero;


		//	LegLeft to BodyLeft
		if (node.Offset > 0.0f && node.Offset <= 0.2f)
		{
			node.CalcNodePosition(
				LegLeft.x, BodyLeft.y,
				leftXRadius, leftYRadius,
				(relativePosToVert + 2.0f) * QuarterCircleRadians);
		}
		//	BodyLeft to BodyTop
		else if (node.Offset > 0.2f && node.Offset <= 0.4f)
		{
			node.CalcNodePosition(
				0f, BodyLeft.y,
				BodyLeft.x, BodyTop.y - BodyLeft.y,
				(-relativePosToVert + 1.0f) * QuarterCircleRadians);
		}
		//	BodyTop to BodyRight
		else if (node.Offset > 0.4f && node.Offset <= 0.6f)
		{
			node.CalcNodePosition(
				0f, BodyRight.y,
				BodyRight.x, BodyTop.y - BodyRight.y,
				(relativePosToVert + 0.0f) * QuarterCircleRadians);
		}
		//	BodyRight to FootRight
		else if (node.Offset > 0.6f && node.Offset <= 0.8f)
		{
			node.CalcNodePosition(
				LegRight.x, BodyRight.y,
				rightXRadius, rightYRadius,
				(relativePosToVert - 1.0f) * QuarterCircleRadians);
		}
		//	FootRight to FootLeft
		else // (node.offset > 0.8f && node.offset <= 1.0f)
		{
			node.Position
				= Vector3.Lerp(LegLeft, LegRight, relativePosToVert)
				+ sheepPosition;
		}

		return newNodePos;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// There are 5 vertices in the porridge form
//		0 - tailLeft
//		1 - wheelLeft
//		2 - wheelTop
//		3 - wheelRight
//		4 - tailRight
//	When the form is moving right the nodes move through the vertices in an ascending order 0->4->0
//	When the form is moving lefft the nodes move through the vertices in an descending order 4->0->4
//	As the speed of the form increases: 
//		Form height increases
//		Tail length decreases
//		A tail vertex moves underneath the wheel and the other moves away (maintaining the tail length)
//		Node speed increases
//	As the speed of the form decreases: 
//		Form height decreases
//		Tail length increases
//		The positions of the tail spaced evenly, either side of the wheel, (maintaining the tail length)
//		Node speed decreases
//	Form height, tail length & wheel radius are also affected by the number of nodes

public class PorridgeFormController : FormController
{
	public FormConstraint tailLength;
	public FormConstraint wheelXOffset;
	public FormConstraint wheelYOffset;
	public FormConstraint wheelXRadius;
	public FormConstraint wheelYRadius;

	public Vector3 TailLeft
	{
		get { return formVertices[0]; }
		set { formVertices[0] = value; }
	}
	public Vector3 WheelLeft
	{
		get { return formVertices[1]; }
		set { formVertices[1] = value; }
	}
	public Vector3 WheelTop
	{
		get { return formVertices[2]; }
		set { formVertices[2] = value; }
	}
	public Vector3 WheelRight
	{
		get { return formVertices[3]; }
		set { formVertices[3] = value; }
	}
	public Vector3 TailRight
	{
		get { return formVertices[4]; }
		set { formVertices[4] = value; }
	}

	protected override void UpdateVertexPositions(float offsetDir)
	{
		float absOffsetDir = Mathf.Abs(offsetDir);
		float offsetInvLerp = Mathf.InverseLerp(1f, -1f, offsetDir);

		//	increases as movement decreases
		tailLength.current
			= tailLength.Midpoint(tailLength.max, tailLength.min, absOffsetDir);
		//	increases as movement increases
		wheelXOffset.current
			= wheelXOffset.Midpoint(wheelXOffset.min, wheelXOffset.max, absOffsetDir);
		//	decreases as movement decreases
		wheelYOffset.current
			= wheelYOffset.Midpoint(wheelYOffset.min, wheelYOffset.max, absOffsetDir);
		//	increases as movement decreases
		wheelXRadius.current
			= wheelXRadius.Midpoint(wheelXRadius.max, wheelXRadius.min, absOffsetDir);
		//	decreases as movement decreases	
		wheelYRadius.current
			= wheelYRadius.Midpoint(wheelYRadius.min, wheelYRadius.max, absOffsetDir);

		if (offsetDir < 0)
			wheelXOffset.current *= -1;
		//	The wheel & the furthest tail vertex follows the direction of the players movement
		TailLeft
			= new Vector3(
				Mathf.Lerp(-tailLength.current, 0f, offsetInvLerp) + wheelXOffset.current * 0.5f,
				0f);
		TailRight
			= new Vector3(TailLeft.x + tailLength.current, 0f);
		WheelLeft
			= new Vector3(
				wheelXOffset.current - wheelXRadius.current,
				wheelYOffset.current);
		WheelRight
			= new Vector3(
				wheelXOffset.current + wheelXRadius.current,
				wheelYOffset.current);
		WheelTop
			= new Vector3(
				wheelXOffset.current,
				wheelYOffset.current + wheelYRadius.current);
	}

	protected override void PositionFormVertices()
	{
		tailLength.current = tailLength.min;
		wheelYOffset.current = wheelYOffset.max;
		wheelXRadius.current = wheelXRadius.min;
		wheelYRadius.current = wheelYOffset.max + wheelXRadius.min;

		TailLeft = new Vector3(-tailLength.current * 0.5f, 0f, 0f);
		TailRight = new Vector3(tailLength.current * 0.5f, 0f);
		WheelLeft = new Vector3(-wheelXRadius.current, wheelYOffset.current);
		WheelTop = new Vector3(0f, wheelYOffset.current + wheelYRadius.current);
		WheelRight = new Vector3(wheelXRadius.current, wheelYOffset.current);
	}

	protected override Vector3 CalcNodePosition(FormNode node)
	{
		float relativePosToVert = (node.Offset % 0.2f) * 5;
		Vector3 newNodePos = Vector3.zero;

		//	TailLeft to WheelLeft
		if (node.Offset > 0.0f && node.Offset <= 0.2f)
		{
			node.CalcNodePosition(
				TailLeft.x, WheelLeft.y, 
				TailLeft.x - WheelLeft.x, WheelLeft.y,
				-((relativePosToVert + 1.0f) * QuarterCircleRadians));
		}
		//	WheelLeft to WheelTop
		else if (node.Offset > 0.2f && node.Offset <= 0.4f)
		{
			node.CalcNodePosition(
				wheelXOffset.current, wheelYOffset.current,
				wheelXRadius.current, wheelYRadius.current,
				(-relativePosToVert + 1.0f) * QuarterCircleRadians);
		}
		//	WheelTop to WheelRight
		else if (node.Offset > 0.4f && node.Offset <= 0.6f)
		{
			node.CalcNodePosition(
				wheelXOffset.current, wheelYOffset.current,
				wheelXRadius.current, wheelYRadius.current,
				(-relativePosToVert + 2.0f) * QuarterCircleRadians);
		}
		//	WheelRight to TailRight
		else if (node.Offset > 0.6f && node.Offset <= 0.8f)
		{
			node.CalcNodePosition(
				TailRight.x, WheelRight.y,
				TailRight.x - WheelRight.x, WheelRight.y,
				(relativePosToVert + 2.0f) * QuarterCircleRadians);
		}
		//	TailRight to TailLeft
		else // (nodeOffset > 0.8f && nodeOffset <= 1.0f)
		{
			node.Position
				= Vector3.Lerp(TailRight, TailLeft, relativePosToVert)
				+ sheepPosition;
		}

		return newNodePos;
	}
}

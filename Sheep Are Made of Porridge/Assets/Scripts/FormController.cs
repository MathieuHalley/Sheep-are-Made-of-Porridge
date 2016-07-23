using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class FormConstraint
{
	public float min;
	public float max;
	public float current;
	public AnimationCurve curve;

	public FormConstraint(float min, float max, AnimationCurve curve)
	{
		this.min = Mathf.Min(min, max);
		this.max = Mathf.Max(min, max);
		this.curve = curve;
	}

	public float Evaluate(float time)
	{
		return curve.Evaluate(time);
	}

	public float Midpoint (float time)
	{
		return Midpoint(min, max, time);
	}

	public float Midpoint(float min, float max, float time)
	{
		return (max - min) * curve.Evaluate(time) + min;
	}
}

public abstract class FormController : MonoBehaviour
{
	public static List<FormNode> FormNodes;
	public GameObject formNodePrefab;
	public GameObject sheepHead;
	public int formNodeCount = 20;
	public float nodeSpeed;

	public FormConstraint headXOffset;
	public FormConstraint headYOffset;
	public FormConstraint headAngle;

	protected Vector3[] formVertices;
	protected Rigidbody2D rigidBody;
	protected Vector3 sheepPosition;

	protected const float QuarterCircleRadians = 90f * Mathf.Deg2Rad;

	protected virtual void Start()
	{
		formVertices = new Vector3[5];
		PositionFormVertices();
		CreateFormNodes();
		DistributeFormNodes();
	}

	protected virtual void Update()
	{
		sheepPosition = this.transform.position;
		MoveFormNodes(
			SheepMovementController.Instance.horizontalInput, 
			nodeSpeed * Time.deltaTime);
		RotateFormNodes(
			SheepMovementController.Instance.horizontalInput, 
			nodeSpeed * Time.deltaTime);
		UpdateVertexPositions(
			SheepMovementController.Instance.horizontalInput);
		UpdateSheepHeadPosition(
			SheepMovementController.Instance.horizontalInput);

		for (int i = 1; i < formVertices.Length; ++i)
		{
			Debug.DrawLine(
				formVertices[i - 1] + sheepPosition, 
				formVertices[i] + sheepPosition);
		}
		Debug.DrawLine(
			formVertices[4] + sheepPosition, 
			formVertices[0] + sheepPosition);
	}

	protected void CreateFormNodes()
	{
		FormNodes = new List<FormNode>(formNodeCount);
		GameObject newFormNodeObject;

		while (FormNodes.Count < formNodeCount)
		{
			newFormNodeObject =
				(GameObject)Instantiate(
					formNodePrefab,
					this.transform.position,
					Quaternion.identity);
			newFormNodeObject.transform.parent = this.transform;
			FormNodes.Add(newFormNodeObject.GetComponent<FormNode>());	
		}
	}

	protected void UpdateSheepHeadPosition(float offsetDir)
	{
		float absOffsetDir = Mathf.Abs(offsetDir);

		headXOffset.current
			= Mathf.Lerp(headXOffset.min, headXOffset.max, absOffsetDir);
		headYOffset.current
			= Mathf.Lerp(headYOffset.min, headYOffset.max, absOffsetDir);
		headAngle.current
			= Mathf.Lerp(headAngle.min, headAngle.max, absOffsetDir);

		if (offsetDir < 0)
		{
			headAngle.current *= -1;
			headXOffset.current *= -1;
		}

		sheepHead.transform.position
			= new Vector3(headXOffset.current, headYOffset.current)
			+ sheepPosition;

		sheepHead.transform.localEulerAngles
			= new Vector3(0, 0, -headAngle.current);
	}

	protected void MoveFormNodes(float offsetDir, float offsetDelta)
	{
		offsetDelta %= 1;
		for (int i = 0; i < formNodeCount; ++i)
		{
			MoveFormNode(FormNodes[i], offsetDir, offsetDelta);
		}
	}

	protected void RotateFormNodes(float offsetDir, float offsetDelta)
	{
		offsetDelta %= 1;
		for (int i = 0; i < formNodeCount; ++i)
		{
			FormNodes[i].Rotation
				-= offsetDir * offsetDelta * Random.Range(100, 500);
		}
	}

	protected void MoveFormNode(FormNode node, float offsetDir, float offsetDelta)
	{
		node.SetOffset(node.Offset + offsetDelta * offsetDir);
		CalcNodePosition(node);
	}

	protected void DistributeFormNodes(float offset = 0f)
	{
		offset %= 1f;
		for (int i = 0; i < formNodeCount; ++i)
		{
			FormNodes[i].SetOffset((float)i / formNodeCount + offset);
			CalcNodePosition(FormNodes[i]);
		}
	}

	protected abstract Vector3 CalcNodePosition(FormNode node);
	protected abstract void UpdateVertexPositions(float offsetDir);
	protected abstract void PositionFormVertices();
}


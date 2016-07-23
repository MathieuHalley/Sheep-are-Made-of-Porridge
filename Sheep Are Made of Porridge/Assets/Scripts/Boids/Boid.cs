using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour
{
	public Vector3 velocity = new Vector3();
	public float fieldOfView = 120f;
	public float velocityScale = 5f;
	public float neighbourRadius = 3f;
	public float axisMovementScalarX = 1f;
	public float axisMovementScalarY = 1f;

	// Use this for initialization
	void Start ()
	{
		this.transform.position = Random.insideUnitCircle;
	}
	
	private void Update()
	{
		this.transform.Translate(this.velocity * velocityScale * Time.deltaTime, Space.World);
	}

	public Boid AddInfluence (Vector3 influence)
	{
		this.velocity += influence.normalized;
		this.velocity = this.velocity.normalized;
		print(this.velocity);
		return this;
	}

	public Boid SetFieldOfView(float fieldOfView)
	{
		this.fieldOfView = fieldOfView;
		return this;
	}

	public Boid SetNeighbourRadius(float neighbourRadius)
	{
		this.neighbourRadius = neighbourRadius;
		return this;
	}
}

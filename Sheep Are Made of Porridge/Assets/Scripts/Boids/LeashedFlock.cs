using UnityEngine;
using System.Collections;

public class LeashedFlock : Flock
{
	public GameObject flockHub;
	public float minDist = 0f;
	public float maxDist = 4f;
	public float equilDist = 2f;
	public float k = 0.05f;
	public float b = 0.05f;

	public new void Update()
	{
		ResolveLeashes();
		base.Update();
	}

	public void OnGUI()
	{
	}

	public LeashedFlock SetDistances(float minDist, float maxDist)
	{
		return SetDistances(minDist, maxDist, maxDist - minDist);
	}
	public LeashedFlock SetDistances(float minDist, float maxDist, float equilDist)
	{
		this.minDist = minDist;
		this.maxDist = maxDist;
		this.equilDist = equilDist;
		return this;
	}

	public LeashedFlock SetLeashTightness(float k)
	{
		this.k = k;
		return this;
	}

	public LeashedFlock SetLeashDampening(float b)
	{
		this.b = b;
		return this;
	}

	public void ResolveLeashes()
	{
		Rigidbody2D hubBody = flockHub.GetComponent<Rigidbody2D>();
		bool hubHasRigidbody = (hubBody != null) ? true : false;
		
		foreach (Boid boid in boids)
		{
			//= -k(|x|-d)(x/|x|) - bv
			Vector3 local = boid.transform.position - flockHub.transform.position;
			Vector3 dir = local.normalized;
			float dist = local.magnitude;
			Vector3 vel = boid.velocity;
			Vector3 influence = 
			//	-(k * local) - (b * vel);
				-(k * (dist-equilDist)*(dir/dist)) - (b * vel);
			//			if (hubHasRigidbody)
			//				vel -= (Vector3)hubBody.velocity;
			if (dist > Mathf.Max(equilDist, maxDist) || dist < Mathf.Min(equilDist, minDist))
//				boid.AddInfluence(influence);
			Debug.DrawRay(boid.transform.position, influence.normalized,Color.red);
			Debug.DrawRay(boid.transform.position, dir.normalized, Color.green);
			Debug.DrawLine(boid.transform.position,flockHub.transform.position);
		}
	}
}

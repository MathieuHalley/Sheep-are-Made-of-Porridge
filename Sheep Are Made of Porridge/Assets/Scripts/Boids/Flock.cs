using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Flock : MonoBehaviour
{
	public GameObject boidPrefab;
	public List<Boid> boids;
	public float cohesionWeight;
	public float alignmentWeight;
	public float separationWeight;
	public uint boidCount;

	private List<Boid> localBoids;
	private System.Func<Vector3, Vector3, float> clockwiseDistance = (v1, v2) => { return (-v1.x * v2.y + v1.y * v2.x); };

	private void Start()
	{
		this.boids = new List<Boid>();
		foreach (GameObject boidGO in GameObject.FindGameObjectsWithTag("Boid"))
			this.boids.Add(boidGO.GetComponent<Boid>());
		if (this.boids.Count > 0)
			boidCount = (uint)this.boids.Count;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			boidCount++;
			UpdateBoidCount();
		}

		UpdateFlockDirection();
		Vector3 influencePoint =
		Camera.main.ScreenToWorldPoint(
			new Vector3(
				Input.mousePosition.x,
				Input.mousePosition.y,
				-Camera.main.transform.position.z));

		if (Input.GetMouseButton(0))
		{
			foreach (Boid boid in boids)
			{
				if ((influencePoint - boid.transform.position).sqrMagnitude < boid.neighbourRadius)
					boid.AddInfluence(-(influencePoint - boid.transform.position));
			}
		}
		else if (Input.GetMouseButton(1))
		{
			foreach (Boid boid in boids)
			{
				if ((influencePoint - boid.transform.position).sqrMagnitude < boid.neighbourRadius)
					boid.AddInfluence((influencePoint - boid.transform.position));
			}
		}
	}

	public void UpdateFlockDirection()
	{
		Vector3 cohesionDir, separationDir;
		MovementInfluenceCollection movementInfluences = new MovementInfluenceCollection();

		foreach (Boid boid in this.boids)
		{
			if (boid.transform == null)
				continue;

			Vector3 centerMass = Vector3.zero;
			Vector3 alignmentDir = Vector3.zero;
			List<Boid> neighbours = GetNeighboursWithinArc(this.boids, boid, boid.fieldOfView).ToList();
			int neighbourCount = neighbours.Count;

			print("neighbourCount " + neighbourCount);

			//	establish the values of centerMass of & alignment
			foreach (Boid neighbour in neighbours)
			{
				centerMass += neighbour.transform.position;
				alignmentDir += neighbour.velocity;
			}

			centerMass /= neighbourCount;
			alignmentDir /= neighbourCount;

			//	Calculate the direction force needs to be applied to the current boid
			cohesionDir = (centerMass - boid.transform.position).normalized;
			separationDir = -cohesionDir;

			print(cohesionDir + " "+ alignmentDir + " " + separationDir);

			movementInfluences.AddInfluence(cohesionDir, cohesionWeight);
			movementInfluences.AddInfluence(alignmentDir, alignmentWeight);
			movementInfluences.AddInfluence(separationDir, separationWeight);

			boid.AddInfluence(movementInfluences.CalculateTotalInfluence());

			movementInfluences.ClearInfluences();
		}
	}

	public IEnumerable<Boid> GetNeighboursWithinArc(List<Boid> boids, Boid boid, float arcDegrees = float.PositiveInfinity)
	{
		if (arcDegrees >= 360f)
			return GetNeighbours(boids, boid);

		Vector3 arcStart = Quaternion.Euler(0, 0, -arcDegrees * 0.5f) * boid.velocity.normalized;
		Vector3 arcEnd = Quaternion.Euler(0, 0, arcDegrees * 0.5f) * boid.velocity.normalized;

		return GetNeighbours(boids, boid)
			.Where(x => clockwiseDistance(arcStart, x.transform.position) < 0)
			.Where(x => clockwiseDistance(arcEnd, x.transform.position) > 0);
	}

	public bool PointIsWithinArc(Boid boid, Vector3 point, float arcDegrees = float.PositiveInfinity)
	{
		Vector3 arcStart = Quaternion.Euler(0, 0, -arcDegrees * 0.5f) * boid.velocity.normalized;
		Vector3 arcEnd = Quaternion.Euler(0, 0, arcDegrees * 0.5f) * boid.velocity.normalized;

		if (clockwiseDistance(arcStart, point) < 0 && clockwiseDistance(arcEnd, point) > 0)
			return true;
		else
			return false;
	}

	public IEnumerable<Boid> GetNeighbours(List<Boid> boids, Boid boid)
	{
		return boids.Where(x => (x.transform.position - boid.transform.position).sqrMagnitude < boid.neighbourRadius);
	}

	private void UpdateBoidCount()
	{
		while (this.boids.Count != boidCount)
		{
			if (this.boids.Count < boidCount)
			{
				GameObject newBoidGO = (GameObject)Instantiate(boidPrefab);
				this.boids.Add(newBoidGO.GetComponent<Boid>());
			}
			else if (this.boids.Count > boidCount)
			{
				this.boids.Remove(this.boids.Last());
			}
		}
	}
}
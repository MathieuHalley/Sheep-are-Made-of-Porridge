using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
///		A system that stores a collection of anonymous influences upon an object's movement. Influences can be added individually and their sum can be calculated
///		AddInfluence
///			Add a new influence to the collection
///		CalculateTotalInfluenceAndClear
///			Calculate & return the sum of movement influences & then clear the collection
///		CalculateTotalInfluence
///			Calculate & return the sum of movement influences
///		ClearInfluences
///			Clear the collection of movement influences
/// </summary>
public class MovementInfluenceCollection
{
	/// <summary>
	///		A simple struct to describe the direction & scale of an influence upon an object's movement
	///		direction - The direction of the influence. This is permitted to be non-normalised to allow for influences that are scaled relative to each other
	///		multiplier - A multiplier for the influence direction. Default = 1f
	/// </summary>
	private struct MovementInfluence
	{
		public Vector3 direction;
		public float multiplier;

		public MovementInfluence(Vector3 direction, float multiplier = 1f)
		{
			this.direction = direction;
			this.multiplier = multiplier;
		}
	}

	private Queue<MovementInfluence> movementInfluenceQueue;

	public MovementInfluenceCollection()
	{
		movementInfluenceQueue = new Queue<MovementInfluence>();
	}

	public void AddInfluence(Vector3 direction, float multiplier = 1f)
	{
		movementInfluenceQueue.Enqueue(new MovementInfluence(direction, multiplier));
	}

	public Vector3 CalculateTotalInfluenceAndClear()
	{
		Vector3 influencedMovement = CalculateTotalInfluence();

		ClearInfluences();

		return influencedMovement;
	}

	public Vector3 CalculateTotalInfluence()
	{
		Vector3 influencedMovement = new Vector3();

		foreach (MovementInfluence influence in movementInfluenceQueue)
			influencedMovement += influence.direction * influence.multiplier;

		return influencedMovement;
	}

	public void ClearInfluences()
	{
		movementInfluenceQueue = new Queue<MovementInfluence>();
	}
}

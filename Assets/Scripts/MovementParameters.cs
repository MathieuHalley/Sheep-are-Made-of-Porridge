using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public class MovementParameters
	{
		[SerializeField] [UsedImplicitly] private float _accelerationForce;
		[SerializeField] [UsedImplicitly] private float _decelerationForce;
		[SerializeField] [UsedImplicitly] private float _maxVelocity;

		public float AccelerationForce
		{
			get { return _accelerationForce; }
		}

		public float DecelerationForce
		{
			get { return _decelerationForce; }
		}

		public float MaxVelocity
		{
			get { return _maxVelocity; }
		}
	}
}
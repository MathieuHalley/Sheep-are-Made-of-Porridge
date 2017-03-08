using UnityEngine;

[System.Serializable]
public class MovementParameters
{
	[SerializeField]
	private float _accelerationForce;
	[SerializeField]
	private float _decelerationForce;
	[SerializeField]
	private float _maxVelocity;

	public float AccelerationForce { get { return _accelerationForce; } }
	public float DecelerationForce { get { return _decelerationForce; } }
	public float MaxVelocity { get { return _maxVelocity; } }
	public MovementParameters()
	{
		this._accelerationForce = 0f;
		this._decelerationForce = 0f;
		this._maxVelocity = 0f;
	}
}
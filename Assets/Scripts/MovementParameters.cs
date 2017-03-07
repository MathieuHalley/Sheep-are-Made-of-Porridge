using UnityEngine;

[System.Serializable]
public class MovementParameters
{
	[SerializeField]
	private float _force;
	[SerializeField]
	private float _maxVelocity;

	public float Force { get { return _force; } }
	public float MaxVelocity { get { return _maxVelocity; } }
	public MovementParameters()
	{
		this._force = 0f;
		this._maxVelocity = 0f;
	}
}
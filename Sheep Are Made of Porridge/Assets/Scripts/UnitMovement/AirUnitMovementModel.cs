using UnityEngine;
using System;

[System.Serializable]
public class AirUnitMovementParameters : UnitMovementParameters
{
	public float flapForce;
}

public class UnitFlapEventArgs : EventArgs { } 

public interface IAirUnitMovementModel : IUnitMovementModel
{
	event EventHandler<UnitFlapEventArgs> FlapEvent;
	void FlapMovement();
}

public class AirUnitMovementModel : UnitMovementModel, IAirUnitMovementModel
{
	protected new readonly AirUnitMovementParameters mParams;
	public event EventHandler<UnitFlapEventArgs> FlapEvent = (sender, e) => { };

	public AirUnitMovementModel
		(GameObject unitGameObject, AirUnitMovementParameters mParams)
		: base(unitGameObject, mParams)
	{
		this.mParams = mParams;
	}

	public void FlapMovement()
	{
		unitRigidbody2D.AddForce(Vector2.up * mParams.flapForce, ForceMode2D.Impulse);
	}
}

using UnityEngine;

public interface IAirUnitMovementModelFactory
{
	IAirUnitMovementModel Model { get; }
}

public class AirUnitMovementModelFactory : IAirUnitMovementModelFactory
{
	public IAirUnitMovementModel Model { get; private set; }

	public AirUnitMovementModelFactory(GameObject unitGameObject, AirUnitMovementParameters mParams)
	{
		Model = new AirUnitMovementModel(unitGameObject, mParams);
	}
}
using UnityEngine;

public interface IUnitMovementModelFactory
{
	IUnitMovementModel Model { get; }
}

public class UnitMovementModelFactory : IUnitMovementModelFactory
{
	public IUnitMovementModel Model { get; private set; }

	public UnitMovementModelFactory (GameObject unitGameObject, UnitMovementParameters mParams)
	{
		Model = new UnitMovementModel(unitGameObject, mParams);
	}
}

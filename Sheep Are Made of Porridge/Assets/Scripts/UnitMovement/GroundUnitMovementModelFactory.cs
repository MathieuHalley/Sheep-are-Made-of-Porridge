using UnityEngine;

public interface IGroundUnitMovementModelFactory
{
	IGroundUnitMovementModel Model { get; }
}

public class GroundUnitMovementModelFactory : IGroundUnitMovementModelFactory
{
	public IGroundUnitMovementModel Model { get; private set; }

	public GroundUnitMovementModelFactory(GameObject unitGameObject, GroundUnitMovementParameters mParams)
	{
		Model = new GroundUnitMovementModel(unitGameObject, mParams);
		Model.InitializeJumpVelocity();
	}
}
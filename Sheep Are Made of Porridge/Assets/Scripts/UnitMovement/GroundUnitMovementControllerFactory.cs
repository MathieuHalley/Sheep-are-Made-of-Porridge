public interface IGroundUnitMovementControllerFactory
{
	IGroundUnitMovementController Controller { get; }
}

public class GroundUnitMovementControllerFactory
{
	public IGroundUnitMovementController Controller { get; private set; }

	public GroundUnitMovementControllerFactory (IGroundUnitMovementModel model, IGroundUnitMovementView view)
	{
		Controller = new GroundUnitMovementController(model, view);
	}
}
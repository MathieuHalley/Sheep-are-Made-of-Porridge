public interface IUnitMovementControllerFactory
{
	IUnitMovementController Controller { get; }
}

public class UnitMovementControllerFactory
{
	public IUnitMovementController Controller { get; private set; }

	public UnitMovementControllerFactory (IUnitMovementModel model, IUnitMovementView view)
	{
		Controller = new UnitMovementController(model, view);
	}
}

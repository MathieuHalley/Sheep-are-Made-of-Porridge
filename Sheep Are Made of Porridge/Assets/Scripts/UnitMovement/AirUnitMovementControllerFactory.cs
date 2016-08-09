public interface IAirUnitMovementControllerFactory
{
	IAirUnitMovementController Controller { get; }
}

public class AirUnitMovementControllerFactory
{
	public IAirUnitMovementController Controller { get; private set; }

	public AirUnitMovementControllerFactory (IAirUnitMovementModel model, IAirUnitMovementView view)
	{
		Controller = new AirUnitMovementController(model, view);
	}
}

public interface IAirUnitMovementController : IUnitMovementController { }

public class AirUnitMovementController : UnitMovementController, IAirUnitMovementController
{
	protected new readonly IAirUnitMovementModel model;
	protected new readonly IAirUnitMovementView view;

	public AirUnitMovementController (IAirUnitMovementModel model, IAirUnitMovementView view) : base(model, view)
	{
		this.model = model;
		this.view = view;

		view.FlapInputEvent += HandleFlapInput;
		model.FlapEvent += HandleFlapEvent;
	}

	// View events
	protected void HandleFlapInput (object sender, FlapInputEventArgs e)
	{
		model.FlapMovement();
	}

	// Model events
	protected void HandleFlapEvent (object sender, UnitFlapEventArgs e)
	{
		view.IsGrounded = model.CheckIsGrounded();
		view.Velocity = model.Velocity;
	}
}

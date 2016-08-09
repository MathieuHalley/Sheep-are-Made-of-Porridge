public interface IGroundUnitMovementController :  IUnitMovementController { }

public class GroundUnitMovementController : UnitMovementController, IGroundUnitMovementController
{
	protected new readonly IGroundUnitMovementModel model;
	protected new readonly IGroundUnitMovementView view;

	public GroundUnitMovementController (IGroundUnitMovementModel model, IGroundUnitMovementView view) : base(model, view)
	{
		this.model = model;
		this.view = view;

		view.JumpInputEvent += HandleJumpInput;
		model.JumpEvent += HandleJumpEvent;
	}

	// View events
	protected void HandleJumpInput (object sender, JumpInputEventArgs e)
	{
		model.JumpMovement();
	}

	// Model events
	protected void HandleJumpEvent (object sender, UnitJumpEventArgs e)
	{
		view.IsGrounded = model.IsGrounded;
		view.Velocity = model.Velocity;
	}
}
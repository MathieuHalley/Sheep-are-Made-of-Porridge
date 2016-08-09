public interface IUnitMovementController { }

public class UnitMovementController : IUnitMovementController
{
	protected readonly IUnitMovementModel model;
	protected readonly IUnitMovementView view;

	public UnitMovementController (IUnitMovementModel model, IUnitMovementView view)
	{
		this.model = model;
		this.view = view;

		view.MovementInputEvent += HandleMovementInput;
		view.StopMovementInputEvent += HandleStopMovementInput;

		model.IsGroundedChangedEvent += HandleIsGroundedChanged;
		model.MovementEvent += HandleMovementEvent;
		model.MoveStateChangedEvent += HandleMoveStateChanged;
	}

	// View events
	protected void HandleMovementInput (object sender, MovementInputEventArgs e)
	{
		model.CurMoveState = (e.IsRunning) ? MoveState.FastMove : MoveState.Move;
		model.Movement(e.MoveDir);
	}

	protected void HandleStopMovementInput (object sender, StopMovementInputEventArgs e)
	{
		model.StopMovement();
	}

	//	Model events
	protected void HandleIsGroundedChanged (object sender, UnitIsGroundedChangedEventArgs e)
	{
		view.IsGrounded = model.IsGrounded;
	}

	protected void HandleMovementEvent (object sender, UnitMovementEventArgs e)
	{
		view.Velocity = model.Velocity;
	}

	protected void HandleMoveStateChanged (object sender, UnitMoveStateChangedEventArgs e)
	{
		view.CurMoveState = model.CurMoveState;
	}
}
using UnityEngine;

public class GroundUnitMovement : MonoBehaviour
{
	public GroundUnitMovementParameters unitMovementParams;
	private GroundUnitMovementView unitMovementView;

	protected virtual void Awake()
	{
		var modelFactory = new GroundUnitMovementModelFactory(gameObject, unitMovementParams);
		var model = modelFactory.Model;

		var viewFactory = new GroundUnitMovementViewFactory(gameObject);
		var view = viewFactory.View;

		new GroundUnitMovementControllerFactory(model, view);
		unitMovementView = view as GroundUnitMovementView;

	}

	protected void CollectInput()
	{
		unitMovementView.DirInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		unitMovementView.RunInput = Input.GetKey(KeyCode.LeftShift);
		unitMovementView.JumpInput = Input.GetKeyDown(KeyCode.Space);
	}

	protected void FixedUpdate()
	{
		CollectInput();
	}
}


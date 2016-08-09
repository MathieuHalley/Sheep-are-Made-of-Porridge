using UnityEngine;

public class AirUnitMovement : MonoBehaviour
{
	public AirUnitMovementParameters unitMovementParams;
	private AirUnitMovementView unitMovementView;

	protected virtual void Awake()
	{
		var modelFactory = new AirUnitMovementModelFactory(gameObject, unitMovementParams);
		var model = modelFactory.Model;

		var viewFactory = new AirUnitMovementViewFactory(gameObject);
		var view = viewFactory.View;

		new AirUnitMovementControllerFactory(model, view);
		unitMovementView = view as AirUnitMovementView;
	}

	protected void CollectInput()
	{
		unitMovementView.DirInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		unitMovementView.RunInput = Input.GetKey(KeyCode.LeftShift);
		unitMovementView.FlapInput = Input.GetKeyDown(KeyCode.Space);
	}

	protected void FixedUpdate()
	{
		CollectInput();
	}
}


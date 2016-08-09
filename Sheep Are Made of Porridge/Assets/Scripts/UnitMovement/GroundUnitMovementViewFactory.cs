using UnityEngine;

public interface IGroundUnitMovementViewFactory
{
	IGroundUnitMovementView View { get; }
}

public class GroundUnitMovementViewFactory : IGroundUnitMovementViewFactory
{
	public IGroundUnitMovementView View { get; private set; }

	public GroundUnitMovementViewFactory(GameObject unitGameObject) 
	{
		View = unitGameObject.GetComponent<GroundUnitMovementView>();
		if (View == null)
			View = unitGameObject.AddComponent<GroundUnitMovementView>();
	}
}
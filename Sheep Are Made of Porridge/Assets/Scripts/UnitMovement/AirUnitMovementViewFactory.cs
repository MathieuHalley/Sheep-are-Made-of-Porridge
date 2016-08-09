using UnityEngine;

public interface IAirUnitMovementViewFactory
{
	IAirUnitMovementView View { get; }
}

public class AirUnitMovementViewFactory : IAirUnitMovementViewFactory
{
	public IAirUnitMovementView View { get; private set; }

	public AirUnitMovementViewFactory(GameObject unitGameObject)
	{
		View = unitGameObject.GetComponent<AirUnitMovementView>();
		if (View == null)
			View = unitGameObject.AddComponent<AirUnitMovementView>();
	}
}
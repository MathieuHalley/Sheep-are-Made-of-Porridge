using UnityEngine;
using System.Collections;

public interface IUnitMovementViewFactory
{
	IUnitMovementView View { get; }
}

public class UnitMovementViewFactory : IUnitMovementViewFactory
{
	public IUnitMovementView View { get; private set; }

	public UnitMovementViewFactory(GameObject unitGameObject)
	{
		View = unitGameObject.GetComponent<UnitMovementView>();
		if (View == null)
			View = unitGameObject.AddComponent<UnitMovementView>();
	}
}

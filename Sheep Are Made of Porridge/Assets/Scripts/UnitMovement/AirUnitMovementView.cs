using UnityEngine;
using System;

public class FlapInputEventArgs : EventArgs { }


public interface IAirUnitMovementView : IUnitMovementView
{
	event EventHandler<FlapInputEventArgs> FlapInputEvent;
	bool FlapInput { get; set; }
}


/// <summary>
///		Takes movement input for air units & triggers input events
/// </summary>
public class AirUnitMovementView : UnitMovementView, IAirUnitMovementView
{
	//	Events
	public event EventHandler<FlapInputEventArgs> FlapInputEvent = (sender, e) => { };

	//	Fields
	protected bool flapInput;

	//	Properties
	public bool FlapInput
	{
		get { return flapInput; }
		set
		{
			if (flapInput != value)
				flapInput = value;
		}
	}


	//	Functions
	protected virtual void Flap()
	{
		if (FlapInput)
			OnFlapInputEvent(new FlapInputEventArgs());
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		Flap();
	}

	//	Event Extensions
	protected virtual void OnFlapInputEvent(FlapInputEventArgs e)
	{
		EventHandler<FlapInputEventArgs> handler = FlapInputEvent;
		if (handler != null)
			handler(this, e);
	}
}

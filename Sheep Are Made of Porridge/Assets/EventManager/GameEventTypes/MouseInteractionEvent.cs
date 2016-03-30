using UnityEngine;
using System.Collections;

[System.Flags]
public enum MouseInteractionEventMessage
{
	OnMouseDown = 1,
	OnMouseDrag = 2,
	OnMouseEnter = 4,
	OnMouseExit = 8,
	OnMouseOver = 16,
	OnMouseUp = 32,
	OnMouseUpAsButton = 64
}

public class MouseInteractionEvent : GameEvent<MouseInteractionEventMessage>
{
//	Fields & Properties
	public static MouseInteractionEvent OnMouseDown
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseDown);
		}
	}
	public static MouseInteractionEvent OnMouseDrag
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseDrag);
		}
	}
	public static MouseInteractionEvent OnMouseEnter
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseEnter);
		}
	}
	public static MouseInteractionEvent OnMouseExit
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseExit);
		}
	}
	public static MouseInteractionEvent OnMouseOver
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseOver);
		}
	}
	public static MouseInteractionEvent OnMouseUp
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseUp);
		}
	}
	public static MouseInteractionEvent OnMouseUpAsButton
	{
		get
		{
			return new MouseInteractionEvent
				(MouseInteractionEventMessage.OnMouseUpAsButton);
		}
	}

//	Constructors
	private MouseInteractionEvent()
		: this(null, default(MouseInteractionEventMessage)) { }

	private MouseInteractionEvent(MouseInteractionEventMessage message)
		: this(null, message) { }

	private MouseInteractionEvent(
		GameObject eventSrc, 
		MouseInteractionEventMessage message)
		: base(typeof(MouseInteractionEvent), eventSrc, message) 
	{ }
}
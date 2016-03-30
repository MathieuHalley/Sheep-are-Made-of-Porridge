using UnityEngine;
using System.Collections;

[System.Flags]
public enum Trigger2DInteractionMessage
{
	OnTriggerEnter2D = 1,
	OnTriggerExit2D = 2,
	OnTriggerStay2D = 4,
}

public class Trigger2DInteractionEvent
	: GameEvent<Trigger2DInteractionMessage, Trigger2DInteractionEvent.Trigger2DData>
{
//	Fields & Properties
	public static Trigger2DInteractionEvent OnTriggerEnter2D
	{
		get
		{
			return new Trigger2DInteractionEvent
				(Trigger2DInteractionMessage.OnTriggerEnter2D);
		}
	}
	public static Trigger2DInteractionEvent OnTriggerExit2D
	{
		get
		{
			return new Trigger2DInteractionEvent
				(Trigger2DInteractionMessage.OnTriggerExit2D);
		}
	}
	public static Trigger2DInteractionEvent OnTriggerStay2D
	{
		get
		{
			return new Trigger2DInteractionEvent
				(Trigger2DInteractionMessage.OnTriggerStay2D);
		}
	}

//	Constructors
	private Trigger2DInteractionEvent() 
		: this(null, default(Trigger2DInteractionMessage), null) { }

	private Trigger2DInteractionEvent(
		Trigger2DInteractionMessage message, 
		Trigger2DData triggerData = null)
		: this(null, message, triggerData) { }

	private Trigger2DInteractionEvent(
		GameObject eventSrc,
		Trigger2DInteractionMessage message,
		Trigger2DData triggerData = null)
		: base(typeof(Trigger2DInteractionEvent), eventSrc, message, triggerData) { }

	/// <summary>
	///		Data format for GameObject Trigger2D collider interactions
	/// </summary>
	public class Trigger2DData : GameObject2DInteractionData
	{
		//	Constructors
		public Trigger2DData() : base() { }

		public Trigger2DData(
			GameObject triggeringGameObject, 
			GameObject otherGameObject)
			: base(triggeringGameObject, otherGameObject) { }

		public Trigger2DData(
			GameObject triggeringGameObject, 
			Collider2D otherCollider)
			: base(triggeringGameObject, otherCollider.gameObject) { }

		public Trigger2DData(
			Collider2D collider, 
			Collider2D otherCollider)
			: base(collider.gameObject, otherCollider.gameObject) { }
	}
}
using UnityEngine;
using System.Collections;

[System.Flags]
public enum Trigger3DInteractionMessage
{
	OnTriggerEnter3D = 1,
	OnTriggerExit3D = 2,
	OnTriggerStay3D = 4,
}

public class Trigger3DInteractionEvent
	: GameEvent<Trigger3DInteractionMessage, Trigger3DInteractionEvent.Trigger3DData>
{
//	Fields & Properties
	public static Trigger3DInteractionEvent OnTriggerEnter3D
	{
		get
		{
			return new Trigger3DInteractionEvent
				(Trigger3DInteractionMessage.OnTriggerEnter3D);
		}
	}
	public static Trigger3DInteractionEvent OnTriggerExit3D
	{
		get
		{
			return new Trigger3DInteractionEvent
				(Trigger3DInteractionMessage.OnTriggerExit3D);
		}
	}
	public static Trigger3DInteractionEvent OnTriggerStay3D
	{
		get
		{
			return new Trigger3DInteractionEvent
				(Trigger3DInteractionMessage.OnTriggerStay3D);
		}
	}
//	Constructors
	private Trigger3DInteractionEvent() 
		: this(null, default(Trigger3DInteractionMessage), null) { }

	private Trigger3DInteractionEvent(
		Trigger3DInteractionMessage message, 
		Trigger3DData triggerData = null)
		: this(null, message, triggerData) { }

	private Trigger3DInteractionEvent(
		GameObject eventSrc,
		Trigger3DInteractionMessage message, 
		Trigger3DData triggerData = null)
		: base(typeof(Trigger3DInteractionEvent), eventSrc, message, triggerData) { }

	/// <summary>
	///		Data format for GameObject Trigger collider interactions
	/// </summary>
	public class Trigger3DData : GameObject3DInteractionData
	{
		//	Constructors
		public Trigger3DData() : base() { }

		public Trigger3DData(
			GameObject triggeringGameObject, 
			GameObject otherGameObject)
			: base(triggeringGameObject, otherGameObject) { }

		public Trigger3DData(
			GameObject triggeringGameObject, 
			Collider otherCollider)
			: base(triggeringGameObject, otherCollider.gameObject) { }

		public Trigger3DData(
			Collider collider, 
			Collider otherCollider)
			: base(collider.gameObject, otherCollider.gameObject) { }

	}
}
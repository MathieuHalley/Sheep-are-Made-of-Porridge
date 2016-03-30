using UnityEngine;
using System.Collections;

[System.Flags]
public enum Collision3DInteractionMessage
{
	OnCollisionEnter3D = 1,
	OnCollisionExit3D = 2,
	OnCollisionStay3D = 4,
}

public class Collision3DInteractionEvent
	: GameEvent<Collision3DInteractionMessage, Collision3DInteractionEvent.Collision3DData>
{
//	Fields & Properties
	public static Collision3DInteractionEvent OnCollisionEnter3D
	{
		get
		{
			return new Collision3DInteractionEvent
				(Collision3DInteractionMessage.OnCollisionEnter3D);
		}
	}
	public static Collision3DInteractionEvent OnCollisionExit3D
	{
		get
		{
			return new Collision3DInteractionEvent
				(Collision3DInteractionMessage.OnCollisionExit3D);
		}
	}
	public static Collision3DInteractionEvent OnCollisionStay3D
	{
		get
		{
			return new Collision3DInteractionEvent
				(Collision3DInteractionMessage.OnCollisionStay3D);
		}
	}

//	Constructors
	private Collision3DInteractionEvent()
		: this(null, default(Collision3DInteractionMessage), null) { }

	private Collision3DInteractionEvent(
		Collision3DInteractionMessage message, 
		Collision3DData collisionData = null)
		: this(null, message, collisionData) { }

	private Collision3DInteractionEvent(
		GameObject eventSrc, 
		Collision3DInteractionMessage message, 
		Collision3DData collisionData = null)
		: base(typeof(Collision3DInteractionEvent), eventSrc, message, collisionData)	{ }

	/// <summary>
	///		Data format for GameObject Rigidbody interactions
	/// </summary>
	public class Collision3DData 
		: GameObject3DInteractionData
	{
	//	Fields & Properties
		public ContactPoint[] Contacts { get; protected set; }
		public Vector3 Impulse { get; protected set; }

	//	Constructors
		public Collision3DData() : base()
		{
			Contacts = new ContactPoint[0];
			Impulse = Vector3.zero;
		}

		public Collision3DData(
			Collider collider,
			Collision collision)
			: this(collider.gameObject, collision) { }

		public Collision3DData(
			GameObject collidingGameObject, 
			Collision collision)
			: base(collidingGameObject, collision.gameObject)
		{
			Contacts = collision.contacts;
			Impulse = collision.impulse;
		}
	}
}
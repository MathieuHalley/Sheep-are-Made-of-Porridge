using UnityEngine;
using System.Collections;

[System.Flags]
public enum Collision2DInteractionMessage
{
	OnCollisionEnter2D = 1,
	OnCollisionExit2D = 2,
	OnCollisionStay2D = 4
}

public class Collision2DInteractionEvent
	: GameEvent<Collision2DInteractionMessage, Collision2DInteractionEvent.Collision2DData>
{
//	Fields & Properties
	public static Collision2DInteractionEvent OnCollisionEnter2D
	{
		get
		{
			return new Collision2DInteractionEvent
				(Collision2DInteractionMessage.OnCollisionEnter2D); 
		}
	}
	public static Collision2DInteractionEvent OnCollisionExit2D
	{
		get
		{
			return new Collision2DInteractionEvent
				(Collision2DInteractionMessage.OnCollisionExit2D);
		}
	}
	public static Collision2DInteractionEvent OnCollisionStay2D
	{
		get
		{
			return new Collision2DInteractionEvent
				(Collision2DInteractionMessage.OnCollisionStay2D);
		}
	}

//	Constructors
	private Collision2DInteractionEvent()
		: this(null, default(Collision2DInteractionMessage), null) { }

	public Collision2DInteractionEvent(
		Collision2DInteractionMessage message, 
		Collision2DData collisionData = null)
		: this(null, message, collisionData) { }

	public Collision2DInteractionEvent(
		GameObject eventSrc, 
		Collision2DInteractionMessage message, 
		Collision2DData collisionData = null)
		: base(typeof(Collision2DInteractionEvent), eventSrc, message, collisionData) { }

	/// <summary>
	///		Data format for GameObject Rigidbody2D interactions
	/// </summary>
	public class Collision2DData : GameObject2DInteractionData
	{
		//	Fields & Properties
		public ContactPoint2D[] Contacts { get; protected set; }

		//	Constructors
		public Collision2DData() : base() 
		{
			Contacts = new ContactPoint2D[0];
		}

		public Collision2DData(
			Collider2D collider,
			Collision2D collision)
			: this(collider.gameObject, collision) { }

		public Collision2DData(
			GameObject collidingGameObject, 
			Collision2D collision)
			: base(collidingGameObject, collision.gameObject)
		{
			Contacts = collision.contacts;
		}
	}
}
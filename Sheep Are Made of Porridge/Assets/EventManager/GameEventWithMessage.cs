using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IGameEvent<TEventMessage>
	: IGameEvent where TEventMessage 
	: struct, System.IComparable, System.IConvertible, System.IFormattable
{
	new TEventMessage EventMessage { get; }
	new bool HasDefaultMessage { get; }
	new IGameEvent SetEventMessage(uint newEventMessage);
	IGameEvent SetEventMessage(TEventMessage newEventMessage);
}

public abstract class GameEvent<TEventMessage>
	: GameEvent, IGameEvent<TEventMessage> where TEventMessage 
	: struct, System.IComparable, System.IConvertible, System.IFormattable
{
//	Fields & Properties
	public new TEventMessage EventMessage { get; protected set; }
	public new bool HasDefaultMessage
	{
		get
		{
			return 
				(EqualityComparer<TEventMessage>.Default
				.Equals(EventMessage, default(TEventMessage))) 
				? true : false;
		}
	}
//	Constructors
	protected GameEvent() : this(typeof(IGameEvent<TEventMessage>), null, default(TEventMessage)) { }

	public GameEvent(System.Type eventTyp, GameObject eventSrc, TEventMessage eventMsg)
		: base(eventTyp, eventSrc)
	{
		if ( !typeof(TEventMessage).IsEnum )
		{
			throw new System.ArgumentException("eventMsg needs to be a valid enum", "eventMsg");
		}
		EventMessage = eventMsg;
	}

	public override IGameEvent SetEventMessage(uint newEventMessage)
	{
		EventMessage = (TEventMessage)System.Convert.ChangeType(newEventMessage, typeof(TEventMessage));
		return this;
	}

	public IGameEvent SetEventMessage(TEventMessage newEventMessage)
	{
		EventMessage = newEventMessage;
		return this;
	}

	public override Dictionary<IGameEvent, EventManager.GameEventDelegate>
		GetCompatibleEvents(Dictionary<IGameEvent, EventManager.GameEventDelegate> eDict)
	{
		Dictionary<IGameEvent, EventManager.GameEventDelegate> compatibleEventsDict;

		compatibleEventsDict =
			(from del in eDict.AsQueryable()
			where EventType == del.Key.GetType()
			where EventSource == null || EventSource == del.Key.EventSource
			where 
				HasDefaultMessage || 
				((uint)System.Convert.ChangeType(EventMessage, typeof(uint)) 
				& (uint)System.Convert.ChangeType(((IGameEvent<TEventMessage>)del.Key).EventMessage, typeof(uint)))
				!= 0
			select del)
			.ToDictionary<KeyValuePair<IGameEvent, EventManager.GameEventDelegate>, 
				IGameEvent, EventManager.GameEventDelegate>(v => v.Key, v => v.Value);

		return compatibleEventsDict;
	}
}



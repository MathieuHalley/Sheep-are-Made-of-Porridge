using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGameEvent<TEventMessage, TEventData>
	: IGameEvent<TEventMessage> where TEventMessage 
	: struct, System.IComparable, System.IConvertible, System.IFormattable
{
	TEventData EventData { get; }

	IGameEvent<TEventMessage, TEventData>
		SetEventData(TEventData newEventData);
}

public abstract class GameEvent<TEventMessage, TEventData>
	: GameEvent<TEventMessage>, IGameEvent<TEventMessage, TEventData>
	where TEventMessage 
	: struct, System.IComparable, System.IConvertible, System.IFormattable
{
//	Fields & Properties
	public TEventData EventData { get; protected set; }

//	Constructors
	protected GameEvent()
		: this(typeof(IGameEvent<TEventMessage, TEventData>),
		null, default(TEventMessage), default(TEventData)) { }

	public GameEvent(
		System.Type eventTyp, GameObject eventSrc, TEventMessage eventMsg, 
		TEventData eventData = default(TEventData))
		: base(eventTyp, eventSrc, eventMsg)
	{
		SetEventData(eventData);
	}

//	Public
	public IGameEvent<TEventMessage, TEventData> 
		SetEventData(TEventData newEventData)
	{
		EventData =
			(EqualityComparer<TEventData>.Default.Equals(newEventData, default(TEventData)))
			? System.Activator.CreateInstance<TEventData>()
			: newEventData;
		return this;
	}

	public override string ToString()
	{
		return base.ToString() +
			" | Event Data: " + 
			((EventData == null) ? "" : " | " + 
				EventData.GetType().ToString() + " : " + EventData.ToString());
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IGameEvent
{
	System.Type EventType { get; }
	GameObject EventSource { get; }
	uint EventMessage { get; }
	bool HasDefaultMessage { get; }
	IGameEvent SetEventSource(GameObject newEventSrc);
	IGameEvent SetEventMessage(uint newEventMessage);
	string ToString();
	Dictionary<IGameEvent, EventManager.GameEventDelegate> GetCompatibleEvents(Dictionary<IGameEvent, EventManager.GameEventDelegate> eDict);
}

public abstract class GameEvent : IGameEvent
{
//	Fields & Properties
	public System.Type EventType { get; protected set; }
	public GameObject EventSource { get; protected set; }
	public uint EventMessage { get; protected set; }
	public bool HasDefaultMessage
	{
		get
		{
			return (EventMessage == default(uint)) ? true : false;
		}
	}

//	Constructors
	protected GameEvent()
		: this(typeof(IGameEvent), null, 0) { }

	public GameEvent(GameObject eventSrc)
		: this(typeof(IGameEvent), eventSrc, 0) { }

	public GameEvent(System.Type eventTyp, GameObject eventSrc, uint eventMsg = 0)
	{
		EventType = eventTyp;
		EventSource = eventSrc;
		EventMessage = eventMsg;
	}

//	Public
	public IGameEvent SetEventSource(GameObject newEventSrc)
	{
		EventSource = newEventSrc;
		return this;
	}

	public virtual IGameEvent SetEventMessage(uint newEventMessage)
	{
		EventMessage = newEventMessage;
		return this;
	}

	public override string ToString()
	{
		return this.GetType().ToString() +
			" | Event Type: " + EventType.ToString() +
			" | Event Source: " + ((EventSource != null)
				? EventSource.ToString() : "Global") +
			" | Event Message: " + EventMessage.ToString();
	}
	public abstract Dictionary<IGameEvent, EventManager.GameEventDelegate> GetCompatibleEvents(Dictionary<IGameEvent, EventManager.GameEventDelegate> eDict);
}
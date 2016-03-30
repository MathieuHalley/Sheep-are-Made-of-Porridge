using UnityEngine;
using System.Collections;

public interface IListenerLookupKey
{
	IGameEvent Event { get; }
	System.Delegate EventDelegate { get; }
	GameObject EventSource { get; }

	string ToString();
}

public class ListenerLookupKey
{
	public IGameEvent Event { get; protected set; }
	public System.Delegate EventDelegate { get; protected set; }
	public GameObject EventSource { get; protected set; }

	public ListenerLookupKey() { }
	public ListenerLookupKey(ListenerLookupKey lookupKey)
		: this(lookupKey.Event, lookupKey.EventDelegate, lookupKey.EventSource) { }
	public ListenerLookupKey(IGameEvent gameEvent, System.Delegate eventDel)
		: this(gameEvent, eventDel, gameEvent.EventSource) { }

	public ListenerLookupKey(IGameEvent evnt, System.Delegate eventDel, GameObject eventSrc)
	{
		Event = evnt;
		EventDelegate = eventDel;
		EventSource = eventSrc;
	}

	public override string ToString()
	{
		return this.GetType().ToString() +
			" | Event Type: " + Event.ToString() +
			" | Event Delegate: " + EventDelegate.ToString() +
			" | Event Source: " + ((EventSource != null) ? EventSource.ToString() : "Global");
	}
}

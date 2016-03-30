using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class EventManager : MonoBehaviour
{
	public static EventManager Instance
	{
		get
		{
			return _Instance 
				?? (_Instance = (EventManager)GameObject
						.FindObjectOfType(typeof(EventManager)));
		}
	}
	private static EventManager _Instance = null;
	
//	Fields & Properties
	//	Public Fields & Properties
	//	override so we don't have the typecast the object

	public delegate void GameEventDelegate(IGameEvent e);
	public delegate void GenericGameEventDelegate<TEvent>(TEvent e)
		where TEvent : IGameEvent;

	public bool LimitQueueProcesing = false;
	public float QueueProcessTime = 0.0f;

	//	Private Fields & Properties
	private Queue eventQueue = new Queue();
	//	Event reference dictionaries
	//	Dictionary for event listeners
	private Dictionary<IGameEvent, GameEventDelegate> eventDelegates
		= new Dictionary<IGameEvent, GameEventDelegate>();
	//	Lookup dictionary for event listeners that will be called an unspecified number of times
	private Dictionary<ListenerLookupKey, GameEventDelegate> listenerLookup
		= new Dictionary<ListenerLookupKey, GameEventDelegate>();
	//	Lookup dictionary for event listeners that will be called a limited number of times
	private Dictionary<ListenerLookupKey, int> limitedListenerLookup
		= new Dictionary<ListenerLookupKey, int>();

//	Functions
	//	Public Functions
	public EventManager AddGlobalEventListener<TEvent>
		(TEvent gameEvent,
		GenericGameEventDelegate<TEvent> del,
		int callCount = -1)
		where TEvent : IGameEvent
	{
		AddEventListener<TEvent>(gameEvent, del, null, callCount);
		return Instance;
	}

	/// <summary>
	///		Add a delegate to listen for an event. 
	///		If callCount isn't '-1' the delegate will 
	///		only be called a certain number of times.
	/// </summary>
	public EventManager AddTargetedEventListener<TEvent>
		(TEvent gameEvent,
		GenericGameEventDelegate<TEvent> del,
		GameObject eventSource,
		int callCount = -1)
		where TEvent : IGameEvent
	{
		gameEvent.SetEventSource(eventSource);
		AddEventListener<TEvent>(gameEvent, del, eventSource, callCount);
		return Instance;
	}

	/// <summary>
	///		Add a delegate to listen for an event. 
	///		If callCount isn't '-1' the delegate will 
	///		only be called a certain number of times.
	/// </summary>
	private EventManager AddEventListener<TEvent>
		(TEvent gameEvent,
		GenericGameEventDelegate<TEvent> del,
		GameObject eventSource,
		int callCount = -1)
		where TEvent : IGameEvent
	{
		GameEventDelegate listenerDelegate;
		ListenerLookupKey lookupKey;
		lookupKey = new ListenerLookupKey(gameEvent, del, eventSource);

		//	If del hasn't been added as a listener to the TEvent 
		//	type of IGameEvent yet, add it as either a global or local listener.
		listenerDelegate = GetEventListener(lookupKey) ?? AddDelegate<TEvent>(lookupKey);

		lookupKey = new ListenerLookupKey(lookupKey.Event, listenerDelegate);

		if ( listenerDelegate != null && callCount > 0 )
		{
			UpdateEventListenerCallCount(lookupKey, callCount);
		}
		return Instance;
	}

	/// <summary>
	///		Stops a specific delegate listening to an event.
	/// </summary>
	public EventManager RemoveEventListener<TEvent>
		(TEvent gameEvent, 
		GenericGameEventDelegate<TEvent> del, 
		GameObject eventSource)
		where TEvent : IGameEvent
	{
		RemoveDelegate(new ListenerLookupKey(gameEvent, del, eventSource));
		return Instance;
	}

	/// <summary>
	///		Checks if the delegate is actually listening for this event.
	/// </summary>
	public bool HasEventListener(ListenerLookupKey listenerLookupKey)
	{
		return listenerLookup.ContainsKey(listenerLookupKey);
	}

	/// <summary>
	///		Get the delegate that is listening to the specific IGameEvent
	/// </summary>
	private GameEventDelegate GetEventListener(ListenerLookupKey lookupKey)
	{
		return (HasEventListener(lookupKey)) ? listenerLookup[lookupKey] : null;
	}

	/// <summary>
	///		Attempts to enqueue the event into the current queue. 
	///		The event will only be enqueued if there is a delegate to listen to it.
	/// </summary>
	public EventManager QueueEvent(IGameEvent newEvent)
	{
		Dictionary<IGameEvent, GameEventDelegate> compatibleEventsDict;

		compatibleEventsDict = newEvent.GetCompatibleEvents(eventDelegates);
		print(newEvent.ToString() + "| Compatible Events: " + compatibleEventsDict.Count);

		if ( compatibleEventsDict.Count > 0 )
		{
			eventQueue.Enqueue(newEvent);
//			Debug.LogWarning( "EventManager: QueueEvent succeeded due to 1+ listeners for event: " + newEvent.ToString());
		}
		else
		{
			Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + newEvent.ToString());
			Debug.Break();
		}

		return Instance;
	}

	//	Private Functions
	/// <summary>
	///		Attempt to trigger a IGameEvent as a global and/or local event.
	/// </summary>
	private void TriggerEvent(IGameEvent e)
	{
		ListenerLookupKey lookupKey;
		List<System.Delegate> invocationList = new List<System.Delegate>();
		Dictionary<IGameEvent, GameEventDelegate> compatibleEventsDict;

		compatibleEventsDict = e.GetCompatibleEvents(eventDelegates);

		foreach (KeyValuePair<IGameEvent, GameEventDelegate> del in compatibleEventsDict)
		{
			invocationList.AddRange(del.Value.GetInvocationList());
		}
		if ( invocationList.Count == 0 )
		{
			Debug.LogWarning("EventManager: TriggerEvent failed due to no listeners for event: " + e.ToString());
			return;
		}

		foreach ( GameEventDelegate eventDelegate in invocationList )
		{
			lookupKey = new ListenerLookupKey(e, eventDelegate);

			eventDelegate.Invoke(e);
			//	Decrement the number of of times any limited listeners 
			//	will be called when this event is triggered
			//	Remove listeners that won't be triggered any more
			UpdateEventListenerCallCount(lookupKey, -1);
		}
	}

	/// <summary>
	///		Add a delegate to the collection of active eventDelegates 
	///		listening for local events
	/// </summary>
	private GameEventDelegate AddDelegate<TEvent>(ListenerLookupKey lookupKey)
		where TEvent : IGameEvent
	{
		GameEventDelegate internalDelegate;
		GameEventDelegate tempDel;
		IGameEvent delEvent = lookupKey.Event;

		// Early-out if we've already registered this delegate
		if ( HasEventListener(lookupKey) )
		{
			return null;
		}
		// Create a new non-generic delegate which calls our generic one.
		// This is the delegate we actually invoke.
		internalDelegate = (e) => ((GenericGameEventDelegate<TEvent>)lookupKey.EventDelegate)((TEvent)e);
		listenerLookup[lookupKey] = internalDelegate;

		if ( eventDelegates.TryGetValue(delEvent, out tempDel) )
		{
			tempDel += internalDelegate;
			eventDelegates[delEvent] = tempDel;
		}
		else
		{
			eventDelegates[delEvent] = internalDelegate;
		}

		return internalDelegate;
	}

	/// <summary>
	///		Stops a specific delegate listening to an event, 
	///		potentially only on a specific gameObject.
	/// </summary>
	private EventManager RemoveDelegate(ListenerLookupKey lookupKey)
	{
		IGameEvent delEvent = lookupKey.Event;
		GameEventDelegate internalDelegate;
		GameEventDelegate tempDel;

		if ( listenerLookup.TryGetValue(lookupKey, out internalDelegate) )
		{
			if ( eventDelegates.TryGetValue(delEvent, out tempDel) )
			{
				tempDel -= internalDelegate;
				if ( tempDel == null )
				{
					eventDelegates.Remove(delEvent);
				}
				else
				{
					eventDelegates[delEvent] = tempDel;
				}
			}
			listenerLookup.Remove(lookupKey);
		}

		return this;
	}

	private int UpdateEventListenerCallCount(
		ListenerLookupKey limitedLookupKey, 
		int callCountDelta)
	{
		int newCallCount = -1;

		if ( !limitedListenerLookup.ContainsKey(limitedLookupKey) )
		{
			return -1;
		}

		newCallCount = limitedListenerLookup[limitedLookupKey] + callCountDelta;
		
		if ( newCallCount <= 0 )
		{
			limitedListenerLookup.Remove(limitedLookupKey);
		}
		else
		{
			limitedListenerLookup[limitedLookupKey] = Mathf.Max(newCallCount, -1);
		}

		return newCallCount;
	}

	/// <summary>
	///		Every update cycle the queue is processed, if the queue processing 
	///		is limited, a maximum processing time per update can be set 
	///		after which the events will have to be processed next update loop.
	/// </summary>
	void Update()
	{
		float timer = 0.0f;
		while ( eventQueue.Count > 0 )
		{
			if ( LimitQueueProcesing &&  timer > QueueProcessTime )
			{
				return;
			}

			TriggerEvent(eventQueue.Dequeue() as GameEvent);

			if ( LimitQueueProcesing )
			{
				timer += Time.deltaTime;
			}
		}
	}

	/// <summary>
	///		Clear records offrom all of the global and targeted eventDelegates 
	///		from the event manager
	/// </summary>
	private void RemoveAll()
	{
		eventDelegates.Clear();
		listenerLookup.Clear();
		limitedListenerLookup.Clear();
	}

	private void OnApplicationQuit()
	{
		RemoveAll();
		eventQueue.Clear();
		_Instance = null;
	}
}


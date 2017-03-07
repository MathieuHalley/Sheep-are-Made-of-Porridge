using System;
using UniRx;
using UnityEngine;

/// <summary>
/// Quick event aggregator using UniRx. Always access through the
///     EventAggregator.Aggregator instance.
/// </summary>
public sealed class EventAggregator : IDisposable {
    readonly Subject<object> eventsSubject = new Subject<object>();

    private static volatile EventAggregator _eventAggregator; // double-check locked
    private static object _syncLock = new object();

    // Nobody instantiates but self
    private EventAggregator() {}

    public static EventAggregator Aggregator {
        get {
            if(_eventAggregator == null)
                lock (_syncLock) {
                    if(_eventAggregator == null)
                        _eventAggregator = new EventAggregator();
                }
            return _eventAggregator;
        }
    }

    public IObservable<TEvent> GetEvent<TEvent>() {
        return eventsSubject.Where(evt => evt is TEvent)
            .Select(e => (TEvent)e);
    }

    public void Publish<TEvent>(TEvent evt) {
        eventsSubject.OnNext(evt);
    }

    public void Dispose() {
        eventsSubject.Dispose();
    }
}

public static class EventAggregatorMonoBehaviourExtensions {
    /// <summary>
    /// Listen for published events using UniRx and RxEventAggregator. Example -<para/>
    ///     this.OnEvent<![CDATA[<SomeKindOfEvent>]]>().TakeUntilDestroy(this).Subscribe(OnSomeKindOfEvent);<para/>
    ///     this.OnEvent<![CDATA[<SomeKindOfEvent>]]>().TakeUntilDestroy(this).Subscribe(() => Debug.Log("Some event happened.");
    /// </summary>
    /// <typeparam name="TEvent">Any kind of event</typeparam>
    /// <param name="behaviour">Extension of MonoBehaviour</param>
    /// <returns></returns>
    public static IObservable<TEvent> OnEvent<TEvent>(this MonoBehaviour behaviour) {
        return EventAggregator.Aggregator.GetEvent<TEvent>();
    }
    /// <summary>
    /// Publishes event to EventAggregator, notifying all observers. Example -<para/>
    ///     this.Publish(new SomeKindOfEvent());
    /// </summary>
    /// <param name="behaviour">Extension of MonoBehaviour</param>
    /// <param name="newEvent">Can publish just about any object (though you shouldn't)</param>
    public static void Publish(this MonoBehaviour behaviour, object newEvent) {
        EventAggregator.Aggregator.Publish(newEvent);
    }
}
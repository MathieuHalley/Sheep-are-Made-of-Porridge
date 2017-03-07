
/*/
/// Author: Justin Kivak (Aahz)
/// License: Provided as-is under MIT license. Have fun :)
/// Date: 12/21/2015
/*/

/*/ 
///	Quick event aggregator in Unity using UniRx, with practically any class able to
///     be used as an event. EventAggregator itself is a threadsafe lazy single
///		instance, always available and ready to go. GetEvent<T>() will return an
///		IObservable that can be subscribed to naturally with UniRx.
/// Typical Usage:
///     - Publishing Events
///         EventAggregator.Aggregator.Publish(new SomeKindOfEvent());
/// 
///     - Subscribing to events
///         EventAggregator.Aggregator.GetEvent<SomeKindOfEvent>()
///             .TakeUntilDestroy(this)
///             .Subscribe(OnSomeKindOfEvent);
///
/// Syntactical sugar has been added to Monobehaviours as well via extension methods,
///		making interactions with EventAggregator even easier:
/*/

	public class TestMonoBehaviour : MonoBehaviour {
		[ContextMenu("Test: Pick up Gold coin")]
		private void TestGoldCoinPickup() {
			this.Publish(new CoinPickedUpEvent() { CoinType = CoinType.Gold });
		}
		void Start() {
			// Observe any CoinPickedUpEvents
			this.OnEvent<CoinPickedUpEvent>()
				.TakeUntilDestroy(this)
				.Subscribe(OnCoinPickedUp);
		}

		private void OnCoinPickedUp(CoinPickedUpEvent evt) {
			Debug.LogFormat("Picked up coin: {0}", evt.CoinType.ToString());
		}
	}

/*/
///	It's recommended to designate and create "event" classes specifically meant to
///		encapsulate the event data. Example:
/*/
	public class CoinPickedUpEvent {
		public CoinType CoinType { get; set; }
	}
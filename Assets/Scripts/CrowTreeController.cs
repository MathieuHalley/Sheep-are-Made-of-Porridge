using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class CrowTreeController : MonoBehaviour
{
	[SerializeField]
	private CrowTreeControllerData _data;
	private CrowTreeControllerData Data { get { return _data; } }

	private void Start()
	{
		SheepEnterRangeSubscription();
		SheepInRangeSubscription();
		SheepExitRangeSubscription();
	}

	private IDisposable SheepEnterRangeSubscription()
	{
		return this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(col =>
			{
				Data.IsSheepInRange = true;
				Data.Sheep = col.transform;
			})
			.AddTo(this);
	}

	//	When the sheep is out of range, return all of the crows to the nest
	private IDisposable SheepExitRangeSubscription()
	{
		return this.OnTriggerExit2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(_ =>
			{
				Data.IsSheepInRange = false;
				foreach (GameObject crow in Data.CrowCollection)
				{
					crow.GetComponent<CrowController>()
						.SetTarget(Data.Nest);
				}
			})
			.AddTo(this);
	}

	private IDisposable	SheepInRangeSubscription()
	{
		var sheepIsStillInRange = Observable
			.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(Data.CrowLaunchDelay))
			.Where(_ => Data.IsSheepInRange == true)
			.Select(_ => Unit.Default);

		return this
			.OnTriggerEnter2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Do(collider =>
			{
				Data.IsSheepInRange = true;
				Data.Sheep = collider.transform;
			})
			.Timestamp().Do(ts => Debug.Log("Enter Range!"))
			.Select(_ => Unit.Default)
			.Merge(sheepIsStillInRange)
			.Throttle(TimeSpan.FromSeconds(Data.CrowLaunchDelay))
			.Where(_ => Data.IsSheepInRange == true)
			.Timestamp().Do(ts => Debug.Log("Crow Launched!"))
			.Subscribe(_ =>
			{
				Data.CrowCollection
					.Peek()
					.GetComponent<CrowController>()
					.SetTarget(Data.Sheep.position);
				Data.CrowCollection
					.Enqueue(Data.CrowCollection.Dequeue());
			})
			.AddTo(this);

		//	When the sheep is in range, send the crows after the sheep one by one.
		//return
		//Data.IsSheepInRangeProperty.AsObservable()
		//	.Where(_ => Data.IsSheepInRange == true)
		//	.Merge(Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
		//		.Where(_ => Data.IsSheepInRange == true)
		//		.Select(_ => Data.IsSheepInRange)
		//	)
		//	.Throttle(TimeSpan.FromSeconds(1))
		//	.Subscribe(_ =>
		//	{
		//		Data.CrowCollection
		//			.Peek()
		//			.GetComponent<CrowController>()
		//			.SetTarget(Data.Sheep.position);
		//		Data.CrowCollection
		//			.Enqueue(Data.CrowCollection.Dequeue());
		//	})
		//	.AddTo(this);
	}

}

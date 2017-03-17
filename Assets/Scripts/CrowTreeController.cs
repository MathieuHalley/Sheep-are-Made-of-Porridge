using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CrowTreeController : ReactiveController<CrowTreeControllerData>
{
	private void Awake()
	{
		if (Data.NestPosition == Vector2.zero)
			Data.NestPosition.Set(this.transform.position.x + Data.NestPosition.x, this.transform.position.y + Data.NestPosition.y);
		for (int i = 0; i < Data.CrowCount; i++)
		{
			GameObject newCrow = Instantiate<GameObject>(
				Data.CrowPrefab,
				this.gameObject.transform.position,
				Quaternion.identity,
				this.transform);
			newCrow
				.GetComponent<CrowController>()
				.SetNest(CurPosition + Data.NestPosition);
			Data.CrowCollection.Enqueue(newCrow);
		}
	}

	private void Start()
	{
		SheepEnterRangeSubscription();
		SheepInRangeSubscription();
		SheepExitRangeSubscription();
	}

	private System.IDisposable SheepEnterRangeSubscription()
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
	private System.IDisposable SheepExitRangeSubscription()
	{
		return this.OnTriggerExit2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(_ =>
			{
				Data.IsSheepInRange = false;
				foreach (GameObject crow in Data.CrowCollection)
				{
					crow.GetComponent<CrowController>()
						.ReturnToNest();
				}
			})
			.AddTo(this);
	}

	private System.IDisposable SheepInRangeSubscription()
	{
		var crowLaunchDelay = System.TimeSpan.FromSeconds(Data.CrowLaunchDelay);
		var onSheepIsStillInRange = Observable
			.Timer(System.TimeSpan.Zero, crowLaunchDelay)
			.Where(_ => Data.IsSheepInRange)
			.Select(_ => Unit.Default);

		return this
			.OnTriggerEnter2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Do(collider =>
			{
				Data.IsSheepInRange = true;
				Data.Sheep = collider.transform;
			})
			.Timestamp().Do(_ => Debug.Log("Enter Range!"))
			.Select(_ => Unit.Default)
			.Merge(onSheepIsStillInRange)
			.Throttle(crowLaunchDelay)
			.Where(_ => Data.IsSheepInRange)
			.Timestamp().Do(_ => Debug.Log("Crow Launched!"))
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
	}

}

using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class CrowTreeController : MonoBehaviour
{
	[SerializeField]
	private CrowTreeControllerData _crowTreeControllerData;
	private CrowTreeControllerData Data { get { return _crowTreeControllerData; } }

	private void Start()
	{
		this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(col =>
			{
				Data.IsSheepInRange = true;
				Data.SheepTarget = col.transform;
			})
			.AddTo(this);

		this.OnTriggerExit2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(_ => Data.IsSheepInRange = false)
			.AddTo(this);
		
		Data.IsSheepInRangeProperty.AsObservable()
			.Where(_ => Data.IsSheepInRange == true)
			.Throttle(TimeSpan.FromSeconds(1))
			.Merge(
				Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
				.Where(_ => Data.IsSheepInRange == true)
				.Select(_ => Data.IsSheepInRange)
			)
			.Subscribe(_ =>
			{
				Data.CrowCollection.Peek().GetComponent<CrowController>().SetCrowTarget(Data.SheepTarget.position);
				Data.CrowCollection.Enqueue(Data.CrowCollection.Dequeue());
			})
			.AddTo(this);

		Data.IsSheepInRangeProperty.AsObservable()
			.Where(_ => Data.IsSheepInRange == false)
			.Subscribe(_ =>
			{
				foreach (GameObject crow in Data.CrowCollection)
					crow.GetComponent<CrowController>().SetCrowTarget(Data.NestTarget);
			})
			.AddTo(this);
	}

}

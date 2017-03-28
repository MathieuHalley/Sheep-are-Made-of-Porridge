using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using System.Linq;

public class CrowTreeController : ReactiveController<CrowTreeControllerData>
{
	public ReactiveProperty<bool> IsSheepInRange { get; private set; }

	private void Awake()
	{
		IsSheepInRange = new ReactiveProperty<bool>(false);
		Data.NestCollider = Data.Nest.GetComponent<Collider2D>();
		Data.SheepCollider = Data.Sheep.GetComponent<Collider2D>();
		for (int i = 0; i < Data.CrowCount; i++)
		{
			GameObject crow = Instantiate<GameObject>(
				Data.CrowPrefab,
				Data.Nest.transform.position,
				Quaternion.identity,
				this.transform);
			Data.CrowCollection.Add(crow.GetComponent<CrowController>());
		}
	}

	private void Start()
	{
		SheepInsideRangeSubscription();
		SheepOutsideRangeSubscription();
		foreach (CrowController crow in Data.CrowCollection)
		{
			CrowIsActiveUpdateSubscription(crow);
		}
	}

	private System.IDisposable CrowIsActiveUpdateSubscription(CrowController crow)
	{
		return
			crow.IsFlightActiveProperty
			.DistinctUntilChanged()
			.Subscribe(isActive =>
			{
				if (isActive)
				{
					Data.ActiveCrowCollection.Add(crow);
				}
				else
				{
					Data.ActiveCrowCollection.Remove(crow);
				}
			})
			.AddTo(this);
	}

	private System.IDisposable SheepInsideRangeSubscription()
	{
		int currentCrow = 0;
		return
		this.OnTriggerEnter2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Do(col => 
			{
				Debug.Log("Sheep Enter Range!");
				IsSheepInRange.Value = true;
				Data.Sheep = col.gameObject;
			})
			.Select(_ => new Unit())
			.Merge(Observable
				.Timer(System.TimeSpan.Zero, Data.CrowUpdateDelta)
				.Select(_ => new Unit()))
			.Where(_ => IsSheepInRange.Value == true)
			.Select(crows => Data.CrowCollection[currentCrow])
			.Subscribe(crow =>
			{
				crow.SetFlightPath(
					crow.transform.position,
					Data.SheepCollider.bounds.center);
				crow.EnqueueFlightPath(
					Data.SheepCollider.bounds.center,
					Data.NestCollider.bounds.center);
				Debug.Log("SheepLaunch");
				currentCrow++;
				currentCrow %= Data.CrowCollection.Count;
			})
			.AddTo(this);
	}
	
	//	When the sheep is out of range, return all of the crows to the nest
	private System.IDisposable SheepOutsideRangeSubscription()
	{
		return
		this.OnTriggerExit2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Do(_ =>
			{
				Debug.Log("Sheep Exit Range!");
				IsSheepInRange.Value = false;
			})
			.Select(_ => new Unit())
			.Merge(Observable
				.Timer(System.TimeSpan.Zero, Data.CrowUpdateDelta)
				.Select(_ => new Unit()))
			.Where(_ => IsSheepInRange.Value == false)
			.Subscribe(crows =>
			{
				foreach (CrowController crow in Data.CrowCollection)
				{
					if (crow.CurrentFlightProgress < 0.99f || crow.CurrentFlightLength > 0.01f)
					{
						crow.SetFlightPath(crow.transform.position, Data.NestCollider.bounds.center);
					}
				}
			})
			.AddTo(this);
	}
}

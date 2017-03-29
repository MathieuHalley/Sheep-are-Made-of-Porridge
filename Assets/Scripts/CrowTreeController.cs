using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using System.Linq;

public class CrowTreeController : ReactiveController<CrowTreeControllerData>
{
	private Vector2 NestPosition { get { return Data.NestCollider.bounds.center; } }
	private Vector2 SheepPosition { get { return Data.SheepCollider.bounds.center; } }
	private void Awake()
	{
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
		SheepEnterRangeSubscription();
		SheepExitRangeSubscription();
		CrowUpdateSubscription();
	}

	public void SetChaseSheep(bool chaseSheep)
	{
		Data.ChaseSheep = chaseSheep;
	}

	private System.IDisposable SheepEnterRangeSubscription()
	{
		return
		this.OnTriggerEnter2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Subscribe(collider =>
			{
				Debug.Log("Sheep Enter Range!");
				Data.ChaseSheep = true;
				Data.Sheep = collider.gameObject;
			})
			.AddTo(this);
	}

	//	When the sheep is out of range, return all of the crows to the nest
	private System.IDisposable SheepExitRangeSubscription()
	{
		return
		this.OnTriggerExit2DAsObservable()
			.Where(collider => collider.tag == "Player")
			.Subscribe(collider =>
			{
				Debug.Log("Sheep Exit Range!");
				Data.ChaseSheep = false;
			})
			.AddTo(this);
	}

	private System.IDisposable CrowUpdateSubscription()
	{
		int currentCrow = 0;
		return
		this.OnEnableAsObservable()
			.Merge(CrowUpdateAsObservable())
			.Select(crows => Data.CrowCollection[currentCrow])
			.Subscribe(crow =>
			{
				if (Data.ChaseSheep == true)
				{
					crow.SetFlightPath(crow.transform.position, SheepPosition);
					crow.EnqueueFlightPath(SheepPosition, NestPosition);
				}
				else
				{
					if (crow.CurrentFlightProgress < 0.99f || crow.CurrentFlightLength > 0.01f)
					{
						crow.SetFlightPath(crow.transform.position, NestPosition);
					}
				}
				currentCrow++;
				currentCrow %= Data.CrowCollection.Count;
			})
			.AddTo(this);
	}
	

	private IObservable<Unit> CrowUpdateAsObservable()
	{
		return Observable
			.Timer(System.TimeSpan.Zero, Data.CrowUpdateDelta)
			.Select(_ => new Unit());
	}
}

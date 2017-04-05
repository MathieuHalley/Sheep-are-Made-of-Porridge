using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts
{
	public class CrowTreeController : ReactiveController<CrowTreeControllerData>
	{
		private void Awake()
		{
			for (var i = 0; i < Data.CrowCount; i++)
			{
				var crow = Instantiate(
					Data.CrowPrefab,
					Data.NestObject.transform.position,
					Quaternion.identity,
					transform);
				Data.CrowCollection.Add(crow.GetComponent<CrowController>());
			}
		}

		private void Start()
		{
			SheepEnterRangeSubscription();
			SheepExitRangeSubscription();
		}

		private void OnEnable()
		{
			CrowFlightPathUpdateSubscription();
		}

		private void SheepEnterRangeSubscription()
		{
			this.OnTriggerEnter2DAsObservable()
				.Where(col => col.tag == "Player")
				.Subscribe(col =>
				{
					Debug.Log("Sheep Enter Range!");
					Data.IsChasingSheep.Value = true;
					Data.SheepObject = col.gameObject;
				})
				.AddTo(this);
		}

		//	When the sheep is out of range, return all of the crows to the nest
		private void SheepExitRangeSubscription()
		{
			this.OnTriggerExit2DAsObservable()
				.Where(col => col.tag == "Player")
				.Subscribe(col =>
				{
					Debug.Log("Sheep Exit Range!");
					Data.IsChasingSheep.Value = false;
					Data.SheepObject = null;
				})
				.AddTo(this);
		}

		private void CrowFlightPathUpdateSubscription()
		{
			var currentCrow = 0;
			this.OnEnableAsObservable()
				.Merge(OnCrowLogicUpdateAsObservable())
				.TakeUntilDisable(this)
				.Select(crows => Data.CrowCollection[currentCrow])
				.Subscribe(crow =>
				{
					var sheepTop = Data.SheepPosition + Data.SheepCollider.offset;
					if (Data.IsChasingSheep.Value && Data.SheepController.IsPorridge)
					{
						Debug.Log("sheepTop " + sheepTop);
						crow.SetFlightPath(crow.CurPosition, sheepTop, 30f);
//						crow.EnqueueFlightPath(sheepTop, Data.NestPosition, 30f, -30f);
					}
					else if (crow.CalcuateIsCurrentFlightActive() || Vector2.Distance(crow.CurPosition, Data.NestPosition) > 0.05f)
					{
						crow.SetFlightPath(crow.CurPosition, Data.NestPosition);
					}

					currentCrow++;
					currentCrow %= Data.CrowCollection.Count;
				})
				.AddTo(this);
		}

		public IObservable<Unit> OnCrowLogicUpdateAsObservable()
		{
			return Observable
				.Timer(System.TimeSpan.Zero, Data.CrowLogicUpdateTimeSpan)
				.Select(_ => new Unit());
		}
	}
}
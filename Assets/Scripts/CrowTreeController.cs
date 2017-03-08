using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class CrowTreeController : MonoBehaviour
{
	[SerializeField]
	private CrowTreeControllerData _crowTreeControllerData;

	public CrowTreeControllerData Data { get { return _crowTreeControllerData; } }


	private void Start()
	{
		this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player")
			.Select(_ => Data.NestTarget)
			.Subscribe(OnSheepEnterRange);
		this.OnTriggerExit2DAsObservable()
			.Where(col => col.tag == "Player")
			.Select(_ => Data.NestTarget)
			.Subscribe(OnSheepExitRange);
		this.UpdateAsObservable()
			.Sample(System.TimeSpan.FromSeconds(1))
			.Where(_ =>
				Data.IsSheepInRange == true &&
				Data.CrowCollection.Peek().GetComponent<CrowController>().Data.IsSwooping == false)
			.Select(_ => Data.SheepTarget)
			.Subscribe(LaunchCrow);
	}

	private void OnSheepEnterRange(Vector2 sheepPosition)
	{
		Data.IsSheepInRange = true;
	}

	private void OnSheepExitRange(Vector2 nestPosition)
	{
		Data.IsSheepInRange = false;
		foreach (GameObject crow in Data.CrowCollection)
			UpdateCrowStatus(crow, nestPosition, false);
	}

	private void UpdateCrowStatus(GameObject crow, Vector2 target, bool isSwooping)
	{
		CrowControllerData crowControllerData = crow.GetComponent<CrowController>().Data;
		crowControllerData.Target = target;
		crowControllerData.IsSwooping = isSwooping;
	}

	private void LaunchCrow(Vector2 sheepPosition)
	{
		if (Data.CrowCollection.Peek().GetComponent<CrowController>().Data.IsSwooping == true)
			return;
		UpdateCrowStatus(Data.CrowCollection.Peek(), sheepPosition, true);
		Data.CrowCollection.Enqueue(Data.CrowCollection.Dequeue());
	}
}

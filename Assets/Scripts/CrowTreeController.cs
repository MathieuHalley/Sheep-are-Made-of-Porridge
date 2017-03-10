using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CrowTreeController : MonoBehaviour
{
	[SerializeField]
	private CrowTreeControllerData _crowTreeControllerData;
	private CrowTreeControllerData Data { get { return _crowTreeControllerData; } }

	private void Start()
	{
		this.OnTriggerEnter2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(_ => 
			{
				Data.IsSheepInRange = true;
			}).AddTo(this);
		this.OnTriggerExit2DAsObservable()
			.Where(col => col.tag == "Player")
			.Subscribe(_ =>
			{
				Data.IsSheepInRange = false;
				foreach (GameObject crow in Data.CrowCollection)
				{
					crow.GetComponent<CrowController>().SetCrowTarget(Data.NestTarget);
				}
			}).AddTo(this);

		//	If only the Join or Window operators existed
		//Data.IsSheepInRangeProperty
		//	.AsObservable()
		//	.Where(inRange => inRange == true)
			
		//Data.IsSheepInRangeProperty
		//	.AsObservable()
		//	.Where(inRange => inRange == false)

		Data.IsSheepInRangeProperty
			.Sample(System.TimeSpan.FromSeconds(1))
			.Timestamp()
			.Do(_ => Debug.Log(_.Timestamp.ToString() + " boop"))
			.Subscribe(_ =>
			{
				Data.CrowCollection.Peek().GetComponent<CrowController>().SetCrowTarget(Data.SheepTarget);
				Data.CrowCollection.Enqueue(Data.CrowCollection.Dequeue());
			}).AddTo(this);
	}
}

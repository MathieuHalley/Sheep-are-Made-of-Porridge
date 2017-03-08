using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CrowController : MonoBehaviour
{
	[SerializeField]
	private CrowControllerData _crowControllerData;

	public CrowControllerData Data { get { return _crowControllerData; } }

	private void Start()
	{
		Data.IsSwoopingProperty
			.Where(_ => Data.Target != (Vector2)this.gameObject.transform.position)
			.Subscribe(ChaseTarget);

	}

	private void ChaseTarget(bool IsSwooping)
	{

	}
}

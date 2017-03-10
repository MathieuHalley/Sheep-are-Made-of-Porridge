using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CrowController : MonoBehaviour
{
	[SerializeField]
	private CrowControllerData _crowControllerData;
	private Rigidbody2D _rigidbody;

	private CrowControllerData Data { get { return _crowControllerData; } }

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		this.FixedUpdateAsObservable()
			.Where(_ => Data.Target != (Vector2)this.gameObject.transform.position)
			.Select(_ => Data.Target)
			.Subscribe(ProcessCrowMovement);
	}

	public void SetCrowTarget(Vector2 newTarget)
	{
		Data.Target = newTarget;
	}

	public void ProcessCrowMovement(Vector2 target)
	{
		_rigidbody.MovePosition(
			Vector2.MoveTowards(
				this.transform.position, 
				target, 
				Data.StandardMovementParameters.MaxVelocity
		));
	}
}

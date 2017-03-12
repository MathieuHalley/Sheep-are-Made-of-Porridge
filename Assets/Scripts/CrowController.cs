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
			.Subscribe(ProcessCrowMovement)
			.AddTo(this);
	}

	public void SetCrowTarget(Vector2 newTarget)
	{
		Data.Target = newTarget;
	}

	public void ProcessCrowMovement(Vector2 target)
	{
		Vector2 movementVector = target - (Vector2)this.transform.position;
		Vector2 movementForce = movementVector.normalized;
		movementForce *= Mathf.Abs(Data.StandardMovementParameters.AccelerationForce);
		_rigidbody.AddForce(movementForce, ForceMode2D.Force);
		_rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, Mathf.Min(movementVector.magnitude,Data.StandardMovementParameters.MaxVelocity));
		//_rigidbody.MovePosition(
		//	Vector2.MoveTowards(
		//		this.transform.position,
		//		target,
		//		Data.StandardMovementParameters.MaxVelocity * Time.deltaTime
		//));
	}
}

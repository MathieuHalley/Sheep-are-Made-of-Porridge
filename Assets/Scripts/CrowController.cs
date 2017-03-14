using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CrowController : ReactiveController<CrowControllerData>
{
	[SerializeField]

	private void Start()
	{
		MoveTowardsNestSubscription();
		MoveTowardsTargetSubscription();
		DeactivateAtTargetSubscription();
	}

	public void SetNest(Vector2 nest)
	{
		Data.Nest = nest;
	}

	public void SetTarget(Vector2 target)
	{
		Data.Target = target;
		Data.IsActive = true;
	}

	public void ReturnToNest()
	{
		Data.IsActive = false;
	}

	private System.IDisposable MoveTowardsNestSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => !Data.IsActive)
			.Select(_ => Data.Nest)
			.Subscribe(nest => MoveTowards(nest))
			.AddTo(this);
	}

	private System.IDisposable MoveTowardsTargetSubscription()
	{
		return
		this.FixedUpdateAsObservable()
			.Where(_ => Data.IsActive)
			.Select(_ => Data.Target)
			.Subscribe(target => MoveTowards(target))
			.AddTo(this);
	}

	private System.IDisposable DeactivateAtTargetSubscription()
	{
		return
		Data.IsActiveProperty.AsObservable()
			.Where(isActive => isActive && Data.Target == CurPosition)
			.Subscribe(_ => Data.IsActive = false)
			.AddTo(this);
	}

	private void MoveTowards(Vector2 goal)
	{	
		float targetDistanceFromNest = (Data.Target - Data.Nest).magnitude;
		float currentDistanceFromNest = (CurPosition - Data.Nest).magnitude;
		float percTravelledFromNest = currentDistanceFromNest / targetDistanceFromNest;
		float flightHeight = Vector2.Lerp(
			Data.Target,
			Data.Nest, 
			Data.FlightHeight.Evaluate(percTravelledFromNest)).y;
		float maxVelocity = Data.MovementParameters.MaxVelocity * Time.fixedDeltaTime;
		Rigidbody.MovePosition(
			Vector2.MoveTowards(
				CurPosition, 
				new Vector2(goal.x, flightHeight), 
				maxVelocity));
	}
}

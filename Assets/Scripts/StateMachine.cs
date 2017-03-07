using UnityEngine;
using System;
using System.Collections;

public interface IStateMachine
{
	IState CurrentState { get; }
	IState PreviousState { get; }
	void ChangeState(IState newState);
	bool IsInState(IState state);
	void Update();
}

[Serializable]
public class StateMachine 
	: MonoBehaviour, IStateMachine
{
	[SerializeField]
	public IState CurrentState { get; private set; }
	[SerializeField]
	public IState PreviousState { get; private set; }

	public void OnEnable()
	{
		CurrentState = null;
		PreviousState = null;
	}

	public void ChangeState(IState newState)
	{
		CurrentState.Exit();
		PreviousState = CurrentState;
		newState.Enter(CurrentState);
		CurrentState  = newState;
	}

	public bool IsInState(IState state)
	{
		return (CurrentState == state) ? true : false;
	}

	public void Update()
	{
		if (CurrentState != null)
			CurrentState.Update();
	}
}
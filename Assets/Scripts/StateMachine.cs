using System;
using UnityEngine;

namespace Assets.Scripts
{
	public interface IStateMachine
	{
		IState CurrentState { get; }
		IState PreviousState { get; }
		void ChangeState(IState newState);
		bool IsInState(IState state);
		void Update();
	}

	[Serializable]
	public class StateMachine : MonoBehaviour, IStateMachine
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
			newState.Enter();
			CurrentState = newState;
		}

		public bool IsInState(IState state)
		{
			if (state == null) throw new ArgumentNullException("state");
			return CurrentState == state;
		}

		public void Update()
		{
			if (CurrentState != null) CurrentState.Update();
		}
	}
}
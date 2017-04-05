using System;
using Assets.Scripts.GameSceneManagement;
using UnityEngine;

namespace Assets.Scripts
{
	public enum GState
	{
		Loading,
		MainMenu,
		PauseMenu,
		Play
	}

	public interface IState
	{
		void Enter();
		void Exit();
		void Update();
	}

	[Serializable]
	public class GameState : IState
	{
		public bool IsActive { get; set; }
		public GameScene[] Scenes { get; set; }

		public void Enter()
		{
			GameSceneManager.Instance.LoadGameScenes(Scenes);
			IsActive = true;
		}

		public void Exit()
		{
			if (Scenes != null) GameSceneManager.Instance.UnloadGameScenes(Scenes);
			IsActive = false;
		}

		public virtual void Update()
		{
		}
	}

	public class GameStateManager : MonoBehaviour
	{
		public GState GameState;
		public GameState LoadingState;
		public GameState MenuState;
		public GameState PauseState;
		public PlayState PlayState;
		private GameState _currentState;

		private static GameStateManager _instance;

		public static GameStateManager Instance
		{
			get
			{
				if (_instance != null) return _instance;
				var gameStateManagerGameObject = GameObject.Find("GameStateManager");
				gameStateManagerGameObject = (gameStateManagerGameObject != null)
					? gameStateManagerGameObject
					: new GameObject("GameStateManager");
				_instance = gameStateManagerGameObject.AddComponent<GameStateManager>();
				return _instance;
			}
		}

		private void Awake()
		{
			_currentState = MenuState;
			MenuState.Enter();
		}

		private void Update()
		{
			switch (GameState)
			{
				case GState.Loading:
					ChangeState(LoadingState);
					break;
				case GState.MainMenu:
					ChangeState(MenuState);
					break;
				case GState.PauseMenu:
					ChangeState(PauseState);
					break;
				case GState.Play:
					ChangeState(PlayState);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			_currentState.Update();
		}

		private void ChangeState(GameState newState)
		{
			if (newState.IsActive) return;
			newState.Enter();
			if (_currentState.IsActive) _currentState.Exit();
			_currentState = newState;
		}
	}
}
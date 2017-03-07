using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public enum GState { Loading, MainMenu, PauseMenu, Play }

public interface IState
{
	void Enter(IState oldState);
	void Exit();
	void Update();
}

[Serializable]
public class GameState 
	: IState
{
	public bool active;
	public GameScene[] scenes;

	public void Enter(IState oldState)
	{
		GameSceneManager.Instance.LoadGameScenes(scenes);
		active = true;
	}

	public void Exit()
	{
		GameSceneManager.Instance.UnloadGameScenes(scenes);
		active = false;
	}
	public virtual void Update() { }
}

public class GameStateManager 
	: MonoBehaviour
{
	private static GameStateManager instance;
	public static GameStateManager Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameStateManagerGameObject;
				gameStateManagerGameObject = GameObject.Find("GameStateManager");
				gameStateManagerGameObject = (gameStateManagerGameObject != null) 
					? gameStateManagerGameObject 
					: new GameObject("GameStateManager");
				instance = gameStateManagerGameObject.AddComponent<GameStateManager>();
			}
			return instance;
		}
	}

	public GState gameState;
	public GameState loadingState;
	public GameState menuState;
	public GameState pauseState;
	public PlayState playState;
	private GameState currentState;

	void Awake()
	{
		currentState = menuState;
		menuState.Enter(default(IState));
	}

	void Update()
	{
		switch(gameState)
		{
			case GState.Loading:
				ChangeState(loadingState);
				break;
			case GState.MainMenu:
				ChangeState(menuState);
				break;
			case GState.PauseMenu:
				ChangeState(pauseState);
				break;
			case GState.Play:
				ChangeState(playState);
				break;
		}
		currentState.Update();
	}

	void ChangeState(GameState newState)
	{
		if (!newState.active)
		{
			newState.Enter(currentState);
			if (currentState.active)
				currentState.Exit();
			currentState = newState;
		}
	}
}


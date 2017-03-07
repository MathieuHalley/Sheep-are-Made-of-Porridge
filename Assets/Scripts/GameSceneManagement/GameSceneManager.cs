using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	private static GameSceneManager instance;
	public  static GameSceneManager Instance
	{
		get
		{
			if (instance == null)
			{
				var gameStateManager = GameObject.Find("GameStateManager");
				if (gameStateManager == null)
				{
					gameStateManager = new GameObject("GameStateManager");
				}
				instance = gameStateManager.GetComponent<GameSceneManager>();
				if (instance == null)
				{
					instance = gameStateManager.AddComponent<GameSceneManager>();
				}
			}
			return instance;
		}
	}

	public void LoadGameScenes(GameScene[] scenes)
	{
		for (var i = 0; i < scenes.Length; ++i)
		{
			StartCoroutine(LoadSceneAsync(scenes[i].sceneName));
		}
	}

	public void UnloadGameScenes(GameScene[] scenes)
	{
		for (var i = 0; i < scenes.Length; ++i)
		{
			StartCoroutine(UnloadScene(scenes[i].sceneName));
		}
	}

	IEnumerator LoadSceneAsync(string sceneName)
	{
		if (SceneManager.GetSceneByName(sceneName).isLoaded)
		{
			yield break;
		}
		var sceneLoadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		sceneLoadOp.allowSceneActivation = true;
		while (!sceneLoadOp.isDone)
		{
			yield return null;
		}
	}

	IEnumerator UnloadScene(string sceneName)
	{
		while(!SceneManager.UnloadSceneAsync(sceneName).isDone)
		{
			yield return null;
		}
	}
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameSceneManagement
{
	public class GameSceneManager : MonoBehaviour
	{
		private static GameSceneManager _instance;

		public static GameSceneManager Instance
		{
			get
			{
				if (_instance != null) return _instance;
				var gameStateManager = GameObject.Find("GameStateManager") ?? new GameObject("GameStateManager");
				_instance = gameStateManager.GetComponent<GameSceneManager>() ?? gameStateManager.AddComponent<GameSceneManager>();
				return _instance;
			}
		}

		public void LoadGameScenes(GameScene[] scenes)
		{
			foreach (var scene in scenes) StartCoroutine(LoadSceneAsync(scene.SceneName));
		}

		public void UnloadGameScenes(GameScene[] scenes)
		{
			foreach (var scene in scenes) StartCoroutine(UnloadScene(scene.SceneName));
		}

		private static IEnumerator LoadSceneAsync(string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
				yield break;
			var sceneLoadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			sceneLoadOp.allowSceneActivation = true;
			while (!sceneLoadOp.isDone)
				yield return null;
		}

		private static IEnumerator UnloadScene(string sceneName)
		{
			while (!SceneManager.UnloadSceneAsync(sceneName).isDone)
				yield return null;
		}
	}
}
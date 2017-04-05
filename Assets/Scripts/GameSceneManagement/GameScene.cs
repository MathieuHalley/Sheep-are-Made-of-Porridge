using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameSceneManagement
{
	[Serializable]
	public class GameScene
	{
		private Scene _scene;
		private string _scenePath;
		public string SceneName { get; set; }

		public string ScenePath
		{
			get { return _scenePath; }
			set { _scenePath = value; }
		}

		public Scene Scene
		{
			get { return _scene; }
			set
			{
				_scene = value;
				SceneName = value.name;
			}
		}

		public GameScene()
		{
			ScenePath = "Assets/";
		}

		public GameScene(string sceneName, string scenePath = "Assets/")
		{
			SetScenePath(scenePath);
			SetScene(sceneName);
		}

		public GameScene SetScene(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", "name");
			var foundScene = SceneManager.GetSceneByPath(ScenePath + name + ".unity");
			if (!foundScene.IsValid()) return this;
			Scene = foundScene;
			SceneName = name;
			return this;
		}

		public GameScene SetScene(Scene scene)
		{
			Scene = scene;
			SceneName = scene.name;
			return this;
		}

		public GameScene SetScenePath(string path = "Assets/")
		{
			if (string.IsNullOrEmpty(ScenePath)) throw new ArgumentException("Value cannot be null or empty.", "path");
			ScenePath = path;
			return this;
		}
	}
}
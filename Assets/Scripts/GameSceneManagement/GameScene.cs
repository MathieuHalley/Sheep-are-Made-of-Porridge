using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

[Serializable]
public class GameScene
{
	private Scene  scene;
	public  string sceneName;
	public  string scenePath;

	public GameScene()
	{
		SetScenePath("Assets/");
	}

	public GameScene(string sceneName, string scenePath = "Assets/")
	{
		SetScenePath(scenePath);
		SetScene(sceneName);
	}

	public GameScene SetScene(string sceneName)
	{
		var foundScene = SceneManager.GetSceneByPath(this.scenePath + sceneName + ".unity");
		if (foundScene.IsValid())
		{
			scene          = foundScene;
			this.sceneName = sceneName;
		}
		return this;
	}

	public GameScene SetScene(Scene scene)
	{
		this.scene = scene;
		sceneName  = scene.name;
		return this;
	}

	public GameScene SetScenePath(string scenePath = "Assets/")
	{
		this.scenePath = scenePath;
		return this;
	}

	public Scene GetScene()
	{
		if (this.scene == default(Scene))
		{
			SetScene(sceneName);
		}
		return scene;
	}
}
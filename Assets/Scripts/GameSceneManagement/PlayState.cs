using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

[Serializable]
public class PlayState 
	: GameState
{
	public override void Update()
	{
		GameObject[] rootGOs;
		float hDir = Input.GetAxis("Horizontal");
		for (var i = 0; i < scenes.Length; ++i)
		{
			Scene scene = scenes[i].GetScene();
			if (scene.isLoaded)
			{
				rootGOs = new GameObject[scene.rootCount];
				rootGOs = scene.GetRootGameObjects();
				foreach (var go in rootGOs)
				{
					go.transform.Translate(
						Vector2.right * hDir * (8 / (1 + i)) * Time.deltaTime);
				}
			}
		}
	}
}

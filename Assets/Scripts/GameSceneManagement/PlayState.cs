using System;
using UnityEngine;

namespace Assets.Scripts.GameSceneManagement
{
	[Serializable]
	public class PlayState : GameState
	{
		public override void Update()
		{
			var hDir = Input.GetAxis("Horizontal");
			for (var i = 0; i < Scenes.Length; ++i)
			{
				var scene = Scenes[i].Scene;
				if (!scene.isLoaded) continue;
				foreach (var go in scene.GetRootGameObjects())
				{
					go.transform.Translate(Vector2.right * hDir * (8f / (1 + i)) * Time.deltaTime);
				}
			}
		}
	}
}
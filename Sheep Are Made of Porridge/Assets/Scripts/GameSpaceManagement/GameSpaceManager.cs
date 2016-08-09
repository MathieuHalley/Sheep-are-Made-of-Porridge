using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSpaceManager : MonoBehaviour
{
	Scene managerScene;
	Scene playScene;
	Scene npcScene;
	Scene backScene;
	Scene vistaScene;

	public string playSceneName;
	public string npcSceneName;
	public string backSceneName;
	public string vistaSceneName;

	void Start()
	{
		managerScene = SceneManager.GetActiveScene();

		StartCoroutine(LoadSceneAsync(playSceneName, LoadSceneMode.Additive));
		playScene = SceneManager.GetSceneByName(playSceneName);
		StartCoroutine(LoadSceneAsync(npcSceneName, LoadSceneMode.Additive));
		npcScene = SceneManager.GetSceneByName(npcSceneName);
		StartCoroutine(LoadSceneAsync(backSceneName, LoadSceneMode.Additive));
		backScene = SceneManager.GetSceneByName(backSceneName);
		StartCoroutine(LoadSceneAsync(vistaSceneName, LoadSceneMode.Additive));
		vistaScene = SceneManager.GetSceneByName(vistaSceneName);
	}

	IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode mode)
	{
		AsyncOperation sceneLoadOp =
		SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		sceneLoadOp.allowSceneActivation = true;
		while (!sceneLoadOp.isDone)
			yield return null;
		Debug.Assert(SceneManager.GetSceneByName(sceneName).IsValid());
	}

	void Update()
	{
		float hDir = Input.GetAxis("Horizontal");
		if (playScene.isLoaded)
		{
			GameObject[] gos = new GameObject[playScene.rootCount];
			gos = playScene.GetRootGameObjects();
			foreach(GameObject go in gos)
			{
				go.transform.Translate(Vector2.right * hDir * 8 * Time.deltaTime);
			}
		}
		if (npcScene.isLoaded)
		{
			GameObject[] gos = new GameObject[npcScene.rootCount];
			gos = npcScene.GetRootGameObjects();
			foreach (GameObject go in gos)
			{
				go.transform.Translate(Vector2.right * hDir * 4 * Time.deltaTime);
			}
		}
		if (backScene.isLoaded)
		{
			GameObject[] gos = new GameObject[backScene.rootCount];
			gos = backScene.GetRootGameObjects();
			foreach (GameObject go in gos)
			{
				go.transform.Translate(Vector2.right * hDir * 2 * Time.deltaTime);
			}
		}
		if (vistaScene.isLoaded)
		{
			GameObject[] gos = new GameObject[vistaScene.rootCount];
			gos = vistaScene.GetRootGameObjects();
			foreach (GameObject go in gos)
			{
				go.transform.Translate(Vector2.right * hDir * 1 * Time.deltaTime);
			}
		}
	}

}
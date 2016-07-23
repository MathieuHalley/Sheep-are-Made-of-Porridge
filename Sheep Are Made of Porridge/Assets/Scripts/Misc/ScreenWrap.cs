using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {
	public Vector2 screenBotCorner;
	public Vector2 screenTopCorner;

	// Use this for initialization
	void Start () {
		screenTopCorner = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
		screenBotCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
	}

	// Update is called once per frame
	void Update () {
		if (this.transform.position.x > screenBotCorner.x)
			this.transform.position = new Vector2(screenTopCorner.x, this.transform.position.y);
		else if (this.transform.position.y > screenBotCorner.y)
			this.transform.position = new Vector2(this.transform.position.x, screenTopCorner.y);
		if (this.transform.position.x < screenTopCorner.x)
			this.transform.position = new Vector2(screenBotCorner.x, this.transform.position.y);
		else if (this.transform.position.y < screenTopCorner.y)
			this.transform.position = new Vector2(this.transform.position.x, screenBotCorner.y);
	}
}

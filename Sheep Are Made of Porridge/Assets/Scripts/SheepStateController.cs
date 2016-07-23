using UnityEngine;
using System.Collections;

public enum SheepState
{
	Sheep,
	Porridge
}

public class SheepStateController : MonoBehaviour {

	public SheepState curSheepState;
	public KeyCode porridgeInput = KeyCode.Space;

	public static SheepStateController Instance { get { return instance; } }

	private static SheepStateController instance;

	void Awake()
	{
		if (instance != null && instance != this)
			Destroy(this.gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
		SetSheepState(SheepState.Sheep);
	}

	// Update is called once per frame
	void Update () {
		SheepStateCheck();
	}

	void SheepStateCheck()
	{
		SheepState newSheepState = curSheepState;

		if (Input.GetKeyDown(porridgeInput) || Input.GetKey(porridgeInput))
			newSheepState = SheepState.Porridge;
		else
			newSheepState = SheepState.Sheep;


		if (curSheepState != newSheepState)
			SetSheepState(SheepState.Sheep);
	}

	void SetSheepState(SheepState newSheepState)
	{
		SendMessage("UpdateSheepState", newSheepState, SendMessageOptions.DontRequireReceiver);
		curSheepState = newSheepState;
	}

}

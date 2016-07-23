using UnityEngine;
using System.Collections;

public enum MoveState
{
	Still,
	Walk,
	Run
}

public class SheepMovementController : MonoBehaviour
{
	[System.Serializable]
	public class MovementProperties
	{
		public float runTopSpeed;
		public float walkTopSpeed;
		public float walkTraction;
		public float runTraction;
		public float runAccel;
		public float walkAccel;
		public float runDecel;
		public float walkDecel;
		public float stillDecel;

		public float airControl;
		public float gravityModifier;
	}

	public MoveState curMoveState;
	public KeyCode movementModifier = KeyCode.LeftShift;
	public bool overTopSpeed = false;
	public float maxCursorOffsetDistance;
	public float horizontalInput;
	public MovementProperties sheepMovementProperties;
	public MovementProperties porridgeMovementProperties;

	public static SheepMovementController Instance { get { return instance; } }

	private static SheepMovementController instance;
	private MovementProperties curMovementProperties;
	private SheepStateController sheepStateController;

	void MoveStateCheck()
	{
		bool modifyMovement = (Input.GetKey(movementModifier)) ? true : false;

		MoveState newMoveState = curMoveState;

		if (horizontalInput != 0 && modifyMovement == true)
			newMoveState = MoveState.Run;
		else if (horizontalInput != 0 && modifyMovement == false)
			newMoveState = MoveState.Walk;
		else if (horizontalInput == 0)
			newMoveState = MoveState.Still;

		if (curMoveState != newMoveState)
			SetMoveState(newMoveState);
	}

	void SetMoveState(MoveState newMoveState)
	{
		curMoveState = newMoveState;
		SendMessage("UpdateMoveState", newMoveState, SendMessageOptions.DontRequireReceiver);
	}

	void UpdateSheepState(SheepState newSheepState)
	{
		//	this transition may spread over multiple frames eventually
		switch (newSheepState)
		{
			case SheepState.Porridge:
				UpdateMovementProperties(porridgeMovementProperties);
				break;
			case SheepState.Sheep:
				UpdateMovementProperties(sheepMovementProperties);
				break;
		}
	}

	void UpdateMovementProperties(MovementProperties movementProperties)
	{ 
		this.curMovementProperties = movementProperties;
	}

	void ResolveMovement()
	{
		Vector3 velocity = this.GetComponent<Rigidbody2D>().velocity;
		Vector3 moveDir = ((Mathf.Sign(horizontalInput) == 1) ? Vector3.right : Vector3.left);

		if (Mathf.Abs(horizontalInput) > 0.1f
			&& Mathf.Abs(velocity.x) > 0.001f
			&& Mathf.Sign(horizontalInput) != Mathf.Sign(velocity.x))
		{
			switch (curMoveState)
			{
				case MoveState.Run:
					velocity 
						-= velocity.normalized 
						* curMovementProperties.runTraction 
						* Time.deltaTime;
					break;
				case MoveState.Walk:
				case MoveState.Still:
					velocity 
						-= velocity.normalized 
						* curMovementProperties.walkTraction 
						* Time.deltaTime;
					break;
			}
		}

		if (Mathf.Abs(horizontalInput) > 0.1f)
		{
			switch (curMoveState)
			{
				case MoveState.Run:
					velocity 
						+= moveDir 
						* curMovementProperties.runAccel 
						* Time.deltaTime;
					velocity 
						= SmoothClampVelocityToSpeed(
							velocity, 
							curMovementProperties.runTopSpeed, 
							curMovementProperties.runDecel);
					break;
				case MoveState.Walk:
				case MoveState.Still:
					velocity 
						+= moveDir 
						* curMovementProperties.walkAccel 
						* Time.deltaTime;
					velocity 
						= SmoothClampVelocityToSpeed(
							velocity, 
							curMovementProperties.walkTopSpeed, 
							curMovementProperties.walkDecel);
					break;
			}
		}
		else if (Mathf.Abs(velocity.x) > 0.001f)
		{
			switch (curMoveState)
			{
				case MoveState.Run:
					velocity 
						-= velocity.normalized 
						* curMovementProperties.walkDecel 
						* Time.deltaTime;
					break;
				case MoveState.Walk:
				case MoveState.Still:
					velocity 
						-= velocity.normalized 
						* curMovementProperties.stillDecel 
						* Time.deltaTime;
					break;
			}
		}
		else
			velocity.x = 0f;

		this.GetComponent<Rigidbody2D>().velocity = velocity;
	}

	Vector3 SmoothClampVelocityToSpeed(Vector3 velocity, float topSpeed, float DecelRate)
	{
		float velocityMagnitude = velocity.magnitude;
		float DecelValue;

		overTopSpeed = false;
		if (velocityMagnitude > topSpeed)
		{
			DecelValue = velocityMagnitude - DecelRate * Time.deltaTime;
			velocity = velocity.normalized * Mathf.Min(topSpeed, DecelValue);
			overTopSpeed = true;
		}

		return velocity;
	}

	void Awake()
	{
		if (instance != null && instance != this)
			Destroy(this.gameObject);
		else
			instance = this;
	}

	void Start()
	{
		this.curMovementProperties = new MovementProperties();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2
		cursorPos
			= Camera.main.ScreenToWorldPoint(
				new Vector3(
					Input.mousePosition.x,
					Input.mousePosition.y,
					this.transform.position.z - Camera.main.transform.position.z))
			- this.transform.position;


		if (Input.GetMouseButton(0))
			horizontalInput 
				= cursorPos.normalized.x * Mathf.Min(1f, cursorPos.magnitude / maxCursorOffsetDistance);
		else
			horizontalInput = Input.GetAxis("Horizontal");

		MoveStateCheck();
		ResolveMovement();
	}

	void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 50), this.GetComponent<Rigidbody2D>().velocity.ToString());
		GUI.Label(new Rect(0, 15, 100, 50), (overTopSpeed) ? "Over top speed" : "");
	}
}

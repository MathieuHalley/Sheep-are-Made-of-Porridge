using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///		Default data format for GameObject interactions
/// </summary>
public abstract class GameObjectInteractionData<TCollider, TRigidbody>
{
//	Fields & Properties
	public GameObjectData<TCollider, TRigidbody> GameObjectAData { get; protected set; }
	public GameObjectData<TCollider, TRigidbody> GameObjectBData { get; protected set; }
	public Vector3 RelativeVelocity { get; protected set; }
	public bool IsDefaultValue
	{
		get
		{
			return
			(EqualityComparer<GameObjectInteractionData<TCollider, TRigidbody>>
			.Default.Equals(this, default(GameObjectInteractionData<TCollider, TRigidbody>)))
			? true : false;
		}
	}

//	Constructors
	public GameObjectInteractionData()
	{
		GameObjectAData = new GameObjectData<TCollider, TRigidbody>();
		GameObjectBData = new GameObjectData<TCollider, TRigidbody>();
		RelativeVelocity = Vector3.zero;
	}

	public GameObjectInteractionData(GameObject gameObjA, GameObject gameObjB)
	{
		GameObjectAData = new GameObjectData<TCollider, TRigidbody>(gameObjA);
		GameObjectBData = new GameObjectData<TCollider, TRigidbody>(gameObjB);
		SetRelativeVelocity();	
	}

	protected void SetRelativeVelocity()
	{
		RelativeVelocity =
			(GameObjectAData.rigidbody != null && GameObjectBData.rigidbody != null)
			? GameObjectBData.velocity - GameObjectAData.velocity 
			: Vector3.zero;
	}
}

/// <summary>
///		Default data format for 2D GameObject interactions
/// </summary>
public class GameObject2DInteractionData : GameObjectInteractionData<Collider2D, Rigidbody2D>
{
//	Fields & Properties
	public new GameObject2DData GameObjectAData { get; protected set; }
	public new GameObject2DData GameObjectBData { get; protected set; }

//	Constructors
	public GameObject2DInteractionData() : base() { }

	public GameObject2DInteractionData(GameObject gameObjA, GameObject gameObjB)
		: base(gameObjA, gameObjB) { }
//	Public

	public class GameObject2DData : GameObjectData<Collider2D, Rigidbody2D>
	{
	//	Constructors
		public GameObject2DData() : base() { }

		public GameObject2DData(GameObject go)
			: this(
				go, 
				go.GetComponent<Collider2D>(), 
				go.GetComponent<Rigidbody2D>(), 
				go.transform) { }

		public GameObject2DData(
			GameObject go, 
			Collider2D col, 
			Rigidbody2D rb, 
			Transform t)
			: base(go, col, rb, t) { }
	}
}

/// <summary>
///		Default data format for 3D GameObject interactions
/// </summary>
public class GameObject3DInteractionData : GameObjectInteractionData<Collider, Rigidbody>
{
//	Fields & Properties
	public new GameObject3DData GameObjectAData { get; protected set; }
	public new GameObject3DData GameObjectBData { get; protected set; }

//	Constructors
	public GameObject3DInteractionData() : base() { }

	public GameObject3DInteractionData(GameObject gameObjA, GameObject gameObjB)
		: base(gameObjA, gameObjB) { }
//	Public

	public class GameObject3DData : GameObjectData<Collider, Rigidbody>
	{
	//	Constructors
		public GameObject3DData() : base() { }

		public GameObject3DData(GameObject go)
			: this(
			go, 
			go.GetComponent<Collider>(), 
			go.GetComponent<Rigidbody>(), 
			go.transform) { }

		public GameObject3DData(
			GameObject go, 
			Collider col, 
			Rigidbody rb, 
			Transform t)
			: base(go, col, rb, t) { }
	}
}
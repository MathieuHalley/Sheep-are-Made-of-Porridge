using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectData<TCol, TRb>
{
//	Fields & Properties
	public readonly GameObject gameObject = null;
	public readonly TCol collider = default(TCol);
	public readonly TRb rigidbody = default(TRb);
	public readonly Transform transform = null;
	public readonly Vector3 velocity = Vector3.zero;
	public bool IsDefaultValue 
	{ 
		get 
		{ 
			return
			(EqualityComparer<GameObjectData<TCol, TRb>>
				.Default.Equals(this, default(GameObjectData<TCol, TRb>)))
			? true : false;
		}
	}

//	Constructors
	public GameObjectData() { }

	public GameObjectData(GameObject go)
		: this(
			go, 
			go.GetComponent<TCol>(), 
			go.GetComponent<TRb>(),
			go.transform) { }

	public GameObjectData(GameObject go, TCol col, TRb rb, Transform t)
	{
		gameObject = go;
		collider = col;
		rigidbody = rb;
		transform = t;
		velocity =
			(rb.GetType() == typeof(Rigidbody))
			? (rb as Rigidbody).velocity
			: (rb.GetType() == typeof(Rigidbody2D))
				? (Vector3)(rb as Rigidbody2D).velocity
				: Vector3.zero;
	}
}

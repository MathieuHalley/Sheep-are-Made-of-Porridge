using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReactiveControllerData { }

public class ReactiveController<T> : MonoBehaviour 
	where T : IReactiveControllerData
{
	[SerializeField]
	private T _data;
	protected T Data { get { return _data; } }

	private Rigidbody2D _rigidbody;
	protected Rigidbody2D Rigidbody { get { return _rigidbody; } }
	protected Vector2 CurPosition { get { return (Vector2)this.gameObject.transform.position; } }
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
	}
}

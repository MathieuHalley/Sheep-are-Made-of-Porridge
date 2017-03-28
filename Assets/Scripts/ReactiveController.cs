using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveController<T> : MonoBehaviour 
	where T : ReactiveControllerData
{
	[SerializeField]
	private T _data;
	protected T Data { get { return _data; } }

	private Rigidbody2D _rigidbody;
	public Rigidbody2D Rigidbody
	{
		get { return _rigidbody == null ? _rigidbody = GetComponent<Rigidbody2D>() : _rigidbody; }
		private set { _rigidbody = value; }
	}
	public Vector2 CurPosition { get { return (Vector2)this.gameObject.transform.position; } }
	public Vector3 CurPosition3D { get { return this.gameObject.transform.position; } }

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody2D>();
	}
}

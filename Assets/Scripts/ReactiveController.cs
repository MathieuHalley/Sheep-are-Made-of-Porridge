using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
	public class ReactiveController<T> : MonoBehaviour
		where T : ReactiveControllerData
	{
		[SerializeField] private T _data;

		protected T Data
		{
			get { return _data ?? (_data = default(T)); }
		}

		private Rigidbody2D _rigidbody;

		public Rigidbody2D Rigidbody
		{
			get { return _rigidbody ?? (_rigidbody = GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>()); }
		}

		public Vector2 CurPosition
		{
			get { return gameObject.transform.position; }
			set { gameObject.transform.position = value; }
		}

		public Vector3 CurPosition3D
		{
			get { return gameObject.transform.position; }
			set { gameObject.transform.position = value; }
		}
	}
}
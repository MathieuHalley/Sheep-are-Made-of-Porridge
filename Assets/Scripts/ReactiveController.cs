using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
	public class ReactiveController<T> : MonoBehaviour
		where T : ReactiveControllerData
	{
		[SerializeField] private T _data;
		private Rigidbody2D _rigidbody;

		protected T Data
		{
			get
			{
				if (_data != null) return _data;
				return _data = default(T);
			}
		}

		public Rigidbody2D Rigidbody
		{
			get
			{
				if (_rigidbody != null) return _rigidbody;
				return _rigidbody = gameObject.GetComponent<Rigidbody2D>() ?? gameObject.AddComponent<Rigidbody2D>();
			}
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
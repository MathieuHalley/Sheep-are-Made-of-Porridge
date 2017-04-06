using System;
using System.Collections;
using System.Linq.Expressions;
using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public abstract class ReactiveControllerData
	{
		protected ReactiveProperty<bool> GetBoolReactiveProperty(ref ReactiveProperty<bool> property, bool initialValue)
		{
			return property ?? (property = new ReactiveProperty<bool>(initialValue));
		}

		protected ReactiveProperty<float> GetFloatReactiveProperty(ref ReactiveProperty<float> property, float initialValue)
		{
			return property ?? (property = new ReactiveProperty<float>(initialValue));
		}

		protected Collider2D GetCollider2D(ref Collider2D collider, GameObject gameObject)
		{
			if (collider != null) return collider;
			if (gameObject == null) return null;
			return collider = gameObject.GetComponent<Collider2D>();
		}

		protected T GetReactiveController<T>(ref T controller, GameObject gameObject)
		{
			if (controller != null) return controller;
			if (gameObject != null) return controller = gameObject.GetComponent<T>();
			return default(T);
		}
	}
}

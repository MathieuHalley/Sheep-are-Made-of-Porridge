using UnityEngine;
using System.Collections;

public class EventManagerTest : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		EventManager.Instance
			.AddTargetedEventListener<MouseInteractionEvent>(
				MouseInteractionEvent.OnMouseDown,
				OnMouseUpInteractionEvent,
				this.gameObject)
			.AddTargetedEventListener<MouseInteractionEvent>(
				MouseInteractionEvent.OnMouseUp,
				OnMouseUpInteractionEvent,
				this.gameObject)
			.AddTargetedEventListener<MouseInteractionEvent>(
				MouseInteractionEvent.OnMouseDown,
				OnMouseUpInteractionEvent,
				Camera.main.gameObject);
//			.AddTargetedEventListener<Collision3DInteractionEvent>(
//				Collision3DInteractionEvent.OnCollisionEnter3D,
//				OnCollisionEnter3DEvent, this.gameObject);
	}

	void OnMouseUpInteractionEvent(MouseInteractionEvent me)
	{
		print("BBBOOOOPPP!!! " + me.EventMessage);
	}

	void OnCollisionEnter3DEvent(Collision3DInteractionEvent ce)
	{
		print("CollisisionBoop");
	}

	void OnMouseUp()
	{
		EventManager.Instance.QueueEvent(MouseInteractionEvent.OnMouseUp);
	}

	/*
	void OnMouseInteraction(MouseInteractionEvent me)
	{
		print("OnMouseInteraction" + me.ToString());
		switch ( me.EventMessage )
		{
			case MouseInteractionEventMessage.OnMouseDown:
				print("boop");
				break;
			case MouseInteractionEventMessage.OnMouseOver:
				print("MouseOver");
				break;
			default:
				break;
		}
	}

	void OnMouseOtherInteraction(MouseInteractionEvent me)
	{
		print("OnMouseOtherInteraction" + me.ToString());
		switch ( me.EventMessage )
		{
			case MouseInteractionEventMessage.OnMouseDown:
			print("boop");
			break;
			case MouseInteractionEventMessage.OnMouseOver:
			print("MouseOver");
			break;
			default:
			break;
		}
	}
	 */
}

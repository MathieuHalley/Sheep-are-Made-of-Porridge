using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface ILocationNode<T> : INode where T : struct 
{
	T Position { get; }
	ILocationNode<T> SetPosition(T position);
}

public class LocationNode<T> : Node, ILocationNode<T> where T : struct
{
	protected T position;

	public T Position { get { return this.position; } }
	public LocationNode(LocationNode<T> node) : base(node) { this.position = node.position; }
	public LocationNode(string nodeID) : base(nodeID) { this.position = new T(); }

	public ILocationNode<T> SetPosition(T position)
	{
		this.position = position;
		return this;
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public interface INode
{
	string NodeID { get; }
	HashSet<IArc> Arcs { get; }
	bool IsAccessible { get; }

	INode AddArc(IArc arc);
	INode RemoveArc(IArc arc);
	IEnumerable<IArc> GetInArcs();
	IEnumerable<IArc> GetOutArcs();
	IEnumerable<INode> GetPredecessors();
	IEnumerable<INode> GetSuccessors();
	IEnumerable<INode> GetNeighbours();
	IEnumerable<IArc> GetArcsWithNeighbour(INode neighbour);
	string ToString();
	int GetHashCode();
}
public interface INode<T> : INode
{
	T Data { get; }
	INode<T> SetData(T data);
}

public class Node : INode
{
	private string nodeID;

	protected HashSet<IArc> arcs;

	//	Properties
	public string NodeID { get { return nodeID; } }
	public HashSet<IArc> Arcs { get { return arcs; } }
	public bool IsAccessible { get { return this.arcs.Count != 0; }	}

	//	Constructors
	protected Node()
	{
		this.nodeID = "";
		this.arcs = new HashSet<IArc>();
	}
	public Node(Node node)
	{
		this.nodeID = node.nodeID;
		this.arcs = node.arcs;
	}
	public Node(string nodeID) : this()
	{
		this.nodeID = nodeID;
	}

	public INode AddArc(IArc arc)
	{
		this.arcs.Add(arc);
		return (INode)this;
	}

	public INode RemoveArc(IArc arc)
	{
		this.arcs.Remove(arc);
		if ( arc.HeadNode == this || arc.TailNode == this )
			arc.RemoveNode(this);

		return (INode)this;
	}

	public IEnumerable<IArc> GetInArcs()
	{
		return this.arcs.Where(x => (x.IsDirected == true && x.HeadNode == this)
								 || (x.IsDirected == false && x.Nodes.Contains(this)));
	}

	public IEnumerable<IArc> GetOutArcs()
	{
		return this.arcs.Where(x => (x.IsDirected == true && x.TailNode == this) 
								 || (x.IsDirected == false && x.Nodes.Contains(this)));
	}

	public IEnumerable<INode> GetPredecessors()
	{
		return GetInArcs()
			.SelectMany(x => x.Nodes)
			.Where(y => y != this);
	}

	public IEnumerable<INode> GetSuccessors()
	{
		return GetOutArcs()
			.SelectMany(x => x.Nodes)
			.Where(y => y != this);
	}

	public IEnumerable<INode> GetNeighbours()
	{
		return this.arcs
			.SelectMany(x => x.Nodes)
			.Where(y => y != this);
	}

	public IEnumerable<IArc> GetArcsWithNeighbour(INode neighbour)
	{
		return this.arcs.Where(x => x.Nodes.Contains(neighbour));
	}

	public new string ToString()
	{
		System.Text.StringBuilder nodeString = new System.Text.StringBuilder();
		nodeString
			.Append("Node: " + this.nodeID + " " + GetHashCode())
			.Append(" - Accessible: " + ((this.IsAccessible) ? "Yes" : "No"))
			.Append(" - InArc count: " + GetInArcs().Count())
			.Append(" - OutArc count: " + GetOutArcs().Count())
			.Append(" - Neighbour count: " + GetNeighbours().Count());
		return nodeString.ToString();
	}

	public new int GetHashCode()
	{
		return (int)Mathf.Repeat(base.GetHashCode() * arcs.GetHashCode() * 31, int.MaxValue);
	}
}

public class Node<T> : Node, INode<T>
{
	private T data;

	// Properties
	public T Data { get { return data; } }

	// Constructors
	protected Node() : base() { data = default(T); }
	public Node(string nodeID) : base(nodeID) { data = default(T); }
	public Node(Node<T> node) : base(node) { this.data = node.data; }

	public INode<T> SetData(T data)
	{
		this.data = data;
		return (INode<T>)this;
	}
}

public class NodeComparer : IEqualityComparer<INode>
{
	public static readonly NodeComparer Instance = new NodeComparer();

	private NodeComparer() { }

	public bool Equals(INode x, INode y)
	{
		if (System.Object.ReferenceEquals(x, y))
			return true;

		if (System.Object.ReferenceEquals(x, null) || System.Object.ReferenceEquals(y, null))
			return false;

		return x.GetHashCode() == y.GetHashCode();
	}

	public int GetHashCode(INode obj) { return obj.GetHashCode(); }
}
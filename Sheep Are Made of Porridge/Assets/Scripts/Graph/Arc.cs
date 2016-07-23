using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IArc
{
	INode[] Nodes { get; }
	INode HeadNode { get; }
	INode TailNode { get; }
	float Weight { get; }
	bool IsDirected { get; }
	bool IsTraversable { get; }

	IArc SetWeight(float weight);
	IArc SetDirected(bool directed);
	IArc SetTraversable(bool traversable);
	IArc SetHeadNode(INode headNode);
	IArc SetTailNode(INode tailNode);
	IArc RemoveNode(INode node);
	IArc RemoveHeadNode();
	IArc RemoveTailNode();

	string ToString();
	int GetHashCode();
}

public class Arc : IArc
{
	private INode[]	nodes;
	private float	weight;
	private bool	directed;
	private bool	traversable;

	//	Properties
	public INode[]	Nodes { get { return nodes; } }
	public INode	HeadNode { get { return nodes[0]; } }
	public INode	TailNode { get { return nodes[1]; } }
	public float	Weight { get { return weight; } }
	public bool IsDirected { get { return this.directed; } }
	public bool IsTraversable
	{
		get
		{
			if (this.nodes[0] == null || this.nodes[1] == null)
				this.traversable = false;

			return this.traversable;
		}
	}

	//	Constructors
	public Arc()
	{
		this.nodes = new INode[2];
		this.weight = 1;
		this.directed = false;
		this.traversable = false;
	}
	public Arc(Arc arc)
	{
		this.nodes = arc.nodes;
		this.weight = arc.weight;
		this.directed = arc.directed;
		this.traversable = arc.traversable;
	}
	public Arc(INode headNode, INode tailNode) : this()
	{
		this.nodes = new INode[2] {headNode.AddArc(this), tailNode.AddArc(this)};
		this.traversable = (headNode == null || tailNode == null) ? false : true;
	}

	public IArc SetWeight(float weight)
	{
		this.weight = weight;
		return (IArc)this;
	}

	public IArc SetDirected(bool directed)
	{
		this.directed = directed;
		return (IArc)this;
	}

	public IArc SetTraversable(bool traversable)
	{
		this.traversable = (this.nodes[0] == null || this.nodes[1] == null) ? false : traversable;
		return (IArc)this;
	}

	public IArc SetHeadNode(INode headNode)
	{
		this.nodes[0] = headNode;
		if (!this.nodes[0].Arcs.Contains((IArc)this))
			this.nodes[0].Arcs.Add((IArc)this);
		return (IArc)this;
	}

	public IArc SetTailNode(INode tailNode)
	{
		this.nodes[1] = tailNode;
		if (!this.nodes[1].Arcs.Contains((IArc)this))
			this.nodes[1].Arcs.Add((IArc)this);

		return (IArc)this;
	}

	public IArc RemoveNode(INode node)
	{
		if (node == this.nodes[0])
		{
			this.nodes[0] = null;
			SetTraversable(false);
		}
		else if (node == this.nodes[1])
		{
			this.nodes[1] = null;
			SetTraversable(false);
		}
		return (IArc)this;
	}

	public IArc RemoveHeadNode()
	{
		this.nodes[0] = null;
		SetTraversable(false);
		return (IArc)this;
	}

	public IArc RemoveTailNode()
	{
		this.nodes[1] = null;
		SetTraversable(false);
		return (IArc)this;
	}

	public new string ToString()
	{
		System.Text.StringBuilder status = new System.Text.StringBuilder();
		status.Append("Arc: " + this.GetHashCode());
		if (this.IsDirected)
		{
			status.Append(" - " +
				  "HeadNode: " + ((this.nodes[0] == null) ? "-" : this.nodes[0].ToString()) 
				+ " to "
				+ "TailNode: " + ((this.nodes[1] == null) ? "-" : this.nodes[1].ToString()));
		}
		else
		{
			status.Append(" - Arc Nodes: " 
					+ ((this.nodes[0] == null) ? "-" : this.nodes[0].ToString()) 
					+ " to "
					+ ((this.nodes[1] == null) ? "-" : this.nodes[1].ToString()));
		}
		status
			.Append(" - Traversable: " + ((this.IsTraversable) ? "Yes" : "No"))
			.Append(" - Directed: " + ((this.IsDirected) ? "Yes" : "No"));

		return status.ToString();
	}

	public new int GetHashCode()
	{
		return (int)Mathf.Repeat(
			base.GetHashCode() 
			* (this.nodes[0].GetHashCode() + this.nodes[1].GetHashCode())
			* this.weight * 319,int.MaxValue);
	}
}

class ArcException : System.Exception
{
	public ArcException(string Message) : base(Message) { }
}

public class ArcComparer : IEqualityComparer<IArc>
{
	public static readonly ArcComparer Instance = new ArcComparer();

	private ArcComparer() { }

	public bool Equals(IArc x, IArc y)
	{
		if (System.Object.ReferenceEquals(x, y))
			return true;
		if (System.Object.ReferenceEquals(x, null) || System.Object.ReferenceEquals(y, null))
			return false;
		return x.GetHashCode() == y.GetHashCode();
	}

	public int GetHashCode(IArc obj)
	{
		return obj.GetHashCode();
	}
}
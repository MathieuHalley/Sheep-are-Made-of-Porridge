using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public interface IGraph
{
	List<INode>	Nodes { get; }
	HashSet<IArc> Arcs { get; }
	IGraph AddArc(IArc arc);
	IGraph AddArc(string headNodeID, string tailNodeID);
	IGraph AddArcs(List<IArc> arc);
	IGraph RemoveArc(IArc arc);
	IEnumerable<INode> GetUnaccessibleNodes();
	HashSet<IArc> GetTraversableArcs();
	HashSet<IArc> GetNonTraversableArcs();
	INode FindNodeByID(string nodeID);

	string ToString();
	int GetHashCode();
}

public class Graph : IGraph
{
	//	Use Hashsets instead of Lists?
	private Dictionary<string, INode> nodes;
	private HashSet<IArc>  arcs;

	//	Properties
	public List<INode>	 Nodes { get { return nodes.Values.ToList(); } }
	public HashSet<IArc> Arcs { get { return arcs; } }

	//	Constructers
	public Graph()
	{
		this.arcs = new HashSet<IArc>();
		this.nodes = new Dictionary<string, INode>();
	}
	public Graph(IArc arc) : this()
	{
		AddArc(arc);
	}
	public Graph(INode[] nodes) : this()
	{
		foreach ( INode node in nodes )
			this.nodes.Add(node.NodeID, node);
	}
	public Graph(List<INode> nodes) : this()
	{
		foreach (INode node in nodes)
			this.nodes.Add(node.NodeID, node);
	}
	public Graph(IEnumerable<INode> nodes) : this()
	{
		foreach (INode node in nodes)
			this.nodes.Add(node.NodeID, node);
	}

	public IGraph AddArc(string headNodeID, string tailNodeID)
	{
		IArc arc = new Arc();
		INode headNode = FindNodeByID(headNodeID);
		INode tailNode = FindNodeByID(tailNodeID);

		if (headNode == default(INode))
		{
			headNode = new Node(headNodeID);
			this.nodes.Add(headNode.NodeID, headNode);
		}

		if (tailNode == default(INode))
		{
			tailNode = new Node(tailNodeID);
			this.nodes.Add(tailNode.NodeID, tailNode);
		}
		arc.SetHeadNode(headNode);
		arc.SetTailNode(tailNode);

		return AddArc(arc);
	}
	public IGraph AddArc(IArc arc)
	{
		this.arcs.Add(arc);
		if (this.nodes.ContainsKey(arc.HeadNode.NodeID))
			this.nodes[arc.HeadNode.NodeID] = arc.HeadNode;
		else
			this.nodes.Add(arc.HeadNode.NodeID, arc.HeadNode);

		if (this.nodes.ContainsKey(arc.TailNode.NodeID))
			this.nodes[arc.TailNode.NodeID] = arc.TailNode;
		else
			this.nodes.Add(arc.TailNode.NodeID, arc.TailNode);
		return (IGraph)this;
	}

	public IGraph AddArcs(List<IArc> arcs)
	{
		foreach (IArc arc in arcs)
			AddArc(arc);
		return (IGraph)this;
	}

	public IGraph RemoveArc(IArc arc)
	{
		this.arcs.Remove(arc);
		foreach (INode node in arc.Nodes)
			node.RemoveArc(arc);
		return (IGraph)this;
	}

	private IGraph RemoveNode(INode node)
	{
		this.nodes.Remove(node.NodeID);
		foreach (IArc arc in node.Arcs)
			arc.RemoveNode(node);
		return (IGraph)this;
	}

	public IEnumerable<INode> GetUnaccessibleNodes()
	{
		return this.nodes.Values.Where(x => !x.IsAccessible);
	}

	public HashSet<IArc> GetTraversableArcs()
	{
		return new HashSet<IArc>(arcs.Where(x => x.IsTraversable));
	}

	public HashSet<IArc> GetNonTraversableArcs()
	{
		return new HashSet<IArc>(arcs.Where(x => !x.IsTraversable));
	}

	public INode FindNodeByID(string nodeID)
	{
		if (this.nodes.ContainsKey(nodeID))
			return this.nodes[nodeID];
		else
			Debug.Log(nodeID + " isn't a valid nodeID");
		return default(INode);
	}

	public List<INode> ShortestPathDijkstra(INode startNode, INode endNode)
	{
		List<INode> path = new List<INode>();
		Dictionary<INode, float> dist = new Dictionary<INode, float>(this.nodes.Count);
		HashSet<INode> unvisitedSet = new HashSet<INode>(this.nodes.Values);

		INode currentNode = startNode;
		INode prevNode = startNode;
		foreach (INode node in this.nodes.Values)
		{
			if (node == startNode)
				dist[node] = 0;
			else
				dist[node] = float.PositiveInfinity;
		}

		while (unvisitedSet.Count != 0)
		{
			currentNode = unvisitedSet
				.OrderBy(x => dist[x])
				.First();

			foreach (IArc arc in currentNode.GetOutArcs())
			{
				if (!unvisitedSet.Contains(arc.HeadNode))
					continue;
				if (dist[arc.HeadNode] > dist[currentNode] + arc.Weight)
					dist[arc.HeadNode] = dist[currentNode] + arc.Weight;
			}

			unvisitedSet.Remove(currentNode);
			prevNode = currentNode;
			if (currentNode == startNode)
				continue;
			path.Add(currentNode);
		}

		return path;
	}



	public new string ToString()
	{
		System.Text.StringBuilder graphString = new System.Text.StringBuilder();
		foreach (INode node in this.nodes.Values)
			graphString.Append(node.ToString()).AppendLine();

		return graphString.ToString();
	}

	public string ArcStatusToString()
	{
		System.Text.StringBuilder graphString = new System.Text.StringBuilder();
		foreach (IArc arc in this.arcs.ToList())
			graphString.Append(arc.ToString()).AppendLine();

		return graphString.ToString();
	}

	public new int GetHashCode()
	{
		return (int)Mathf.Repeat(arcs.GetHashCode() * this.nodes.GetHashCode() * 31, int.MaxValue);
	}
}
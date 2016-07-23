using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphTest : MonoBehaviour {

	public Graph g;
	public GameObject nodePrefab;
	List<INode> path;
	public List<GameObject> sheepBits;
	// Use this for initialization
	void Start ()
	{
		List<ILocationNode<Vector2>> nodes = new List<ILocationNode<Vector2>>();
		List<IArc> arcs = new List<IArc>();
		path = new List<INode>();
		g = new Graph();
		nodes.Add(new LocationNode<Vector2>("0"));
		nodes.Add(new LocationNode<Vector2>("1"));
		nodes.Add(new LocationNode<Vector2>("2"));
		nodes.Add(new LocationNode<Vector2>("3"));
		nodes.Add(new LocationNode<Vector2>("4"));
		nodes.Add(new LocationNode<Vector2>("5"));
		nodes.Add(new LocationNode<Vector2>("6"));
		nodes.Add(new LocationNode<Vector2>("7"));
		nodes.Add(new LocationNode<Vector2>("8"));
		nodes.Add(new LocationNode<Vector2>("9"));

		foreach (LocationNode<Vector2> node in nodes)
			node.SetPosition(Random.insideUnitCircle * 10);

		arcs.Add(new Arc().SetHeadNode(nodes[0]).SetTailNode(nodes[1]));
		arcs.Add(new Arc().SetHeadNode(nodes[1]).SetTailNode(nodes[3]));
		arcs.Add(new Arc().SetHeadNode(nodes[2]).SetTailNode(nodes[5]));
		arcs.Add(new Arc().SetHeadNode(nodes[3]).SetTailNode(nodes[7]));
		arcs.Add(new Arc().SetHeadNode(nodes[4]).SetTailNode(nodes[9]));
		arcs.Add(new Arc().SetHeadNode(nodes[5]).SetTailNode(nodes[2]));
		arcs.Add(new Arc().SetHeadNode(nodes[6]).SetTailNode(nodes[2]));
		arcs.Add(new Arc().SetHeadNode(nodes[7]).SetTailNode(nodes[4]));
		arcs.Add(new Arc().SetHeadNode(nodes[8]).SetTailNode(nodes[6]));
		arcs.Add(new Arc().SetHeadNode(nodes[9]).SetTailNode(nodes[8]));

		g.AddArcs(arcs);
		SetArcWeight();

		foreach (INode node in g.Nodes)
		{
			print(node.ToString() + "\n" + node.Arcs.Count);
		}

		foreach (IArc arc in g.Arcs)
		{
			print(arc.ToString());
		}

		path = g.ShortestPathDijkstra(nodes[0], nodes[9]);

		foreach (LocationNode<Vector2> node in g.Nodes)
			sheepBits.Add((GameObject)Instantiate(nodePrefab,node.Position,Quaternion.identity));

		foreach (INode node in path)
			print(node);
	}

	// Update is called once per frame
	void Update ()
	{
		DrawGraph();
	}

	void Jiggle()
	{
		Vector2 jiggleDir;
		for (int i = 0; i < g.Nodes.Count; ++i)
		{
			jiggleDir = Random.insideUnitCircle * Time.deltaTime * 2;
			((LocationNode<Vector2>)g.Nodes[i]).SetPosition(((LocationNode<Vector2>)g.Nodes[i]).Position + jiggleDir);
			sheepBits[i].transform.Translate(jiggleDir);
		}
	}

	void SetArcWeight()
	{
		foreach (IArc arc in g.GetTraversableArcs())
		{
			arc.SetWeight(
				Vector2.Distance(
					((LocationNode<Vector2>)arc.TailNode).Position,
					((LocationNode<Vector2>)arc.HeadNode).Position
				));
		}
	}

	void DrawGraph()
	{
		List<INode> nodes = g.Nodes;
		foreach (IArc arc in g.Arcs)
		{
			Debug.DrawLine(
				((LocationNode<Vector2>)arc.HeadNode).Position,
				((LocationNode<Vector2>)arc.TailNode).Position,
				Color.red);
		}
		for (int i = 1; i < path.Count; ++i)
		{
			Debug.DrawLine(
				((LocationNode<Vector2>)path[i]).Position,
				((LocationNode<Vector2>)path[i - 1]).Position,
				Color.green);
		}
	}
}

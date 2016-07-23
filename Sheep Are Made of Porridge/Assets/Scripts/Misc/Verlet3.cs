// converted to unity with some modifications - mgear - http://unitycoder.com/blog/
// original source: http://www.xbdev.net/physics/Verlet/index.php

using UnityEngine;
using System.Collections;

	// Some helper containers
	struct Constraint2
	{
		  public int   index0;
		  public int   index1;
		  public float restLength;
	};
	
	struct Point
	{
		public Vector3	curPos;
		public Vector3	oldPos;
		public bool		unmovable;
	};

public class Verlet3 : MonoBehaviour {

	private float gravity = -9.8f*0.5f;
	private float softness = 0.01135f*0.80f;
	
	private int		m_numPoints;
    private int		m_numConstraints;
	private Point[] m_points;
	private Vector3 m_offset;
	private float	m_scale;
    private Constraint2[]  m_constraints;
	private Mesh mesh;
	
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		Create();
	}
	
	// Update is called once per frame
	void Update () {
        VerletIntegrate(Time.deltaTime);
        SatisfyConstraints();
		Draw();
	}
	
	void Draw()
	{
		UpdateVerts();
	}

	void VerletIntegrate(float dt)
	{
		Vector3 oldPos;
		Vector3 curPos;
		Vector3 a;

		for (int i=0; i<m_numPoints; i++)
		{
			if (!m_points[i].unmovable)
			{
				oldPos = m_points[i].oldPos;
				curPos = m_points[i].curPos;
				a = new Vector3(0,gravity,0);

//				curPos = 2*curPos - oldPos + a*dt*dt;
				curPos = 1.995f*curPos - 0.995f*oldPos + a*dt*dt;

				m_points[i].oldPos = m_points[i].curPos;
				m_points[i].curPos = curPos;
			}
		}
	}

	void SatisfyConstraints()
	{
		const int numIterations = 1;
		Constraint2 c;
		Vector3 p0;
		Vector3 p1;
		Vector3 delta;
		float len;
		float diff;

		for (int i = 0; i < numIterations; i++)
		{
			for (int k = 0; k < m_numConstraints; k++)
			{
				// Constraint 1 (Floor)
				for (int v = 0; v < m_numPoints; v++)
				{
					if (m_points[v].curPos.y < 0.0f)
						m_points[v].curPos.y = 0.0f;
				}

				// Constraint 2 (Links)
				c = m_constraints[k];
				p0 = m_points[c.index0].curPos;
				p1 = m_points[c.index1].curPos;
				delta = p1-p0;
				len = delta.magnitude;
				diff = (len - c.restLength) / len;
				p0 += delta * softness * diff;
				p1 -= delta * softness * diff;

				if (p0.y>-110)
				{
					m_points[c.index0].curPos=p0;
					m_points[c.index1].curPos=p1;	  
				}

				if (m_points[c.index0].unmovable)
				{
					m_points[c.index0].curPos = m_points[c.index0].oldPos;
				}
				if (m_points[c.index1].unmovable)
				{
					m_points[c.index1].curPos = m_points[c.index1].oldPos;
				}
			}
		}
	}
	
	void UpdateVerts()
	{
		Vector3[] vertices = mesh.vertices;
		for (int i=0; i<(int)m_numPoints; i++)
		{
			vertices[i] = m_points[i].curPos;
		}
		
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
        mesh.RecalculateBounds();
	}
	  
	void Create()
	{
		m_scale	 = 0.3f;

		// get verts count
		Vector3[] vertices = mesh.vertices;
		m_numPoints = vertices.Length;
		
		m_points = new Point[m_numPoints];

		m_numConstraints = (m_numPoints * m_numPoints) - m_numPoints;
		m_constraints = new Constraint2[m_numConstraints];

		Vector3 pos;
		for (int i=0; i<m_numPoints; i++)
		{
			pos = vertices[i];
			m_points[i].curPos = (pos)*m_scale + m_offset;
			m_points[i].oldPos = (pos)*m_scale + m_offset;
			m_points[i].unmovable = false;
		}

		// create constraints
		int c = 0;
		float len;
		for (int i=0; i<m_numPoints; i++)
		{
			for (int k=0; k<m_numPoints; k++)
			{
				if (i!=k)
				{
					len = (m_points[i].curPos - m_points[k].curPos).magnitude;

					m_constraints[c].restLength = len;
					m_constraints[c].index0 = i;
					m_constraints[c].index1 = k;
					c++;
				}
			}
		}
	}
}


using UnityEngine;
using System.Collections;

public class ObliqueCamera : MonoBehaviour {

	bool obliqueCamera;
	Camera cam;
	public float angle = 45.0f;
	public float zScale = 0.5f;
	public float zOffset = 0.0f;

	// Use this for initialization
	void Start () {
		cam = Camera.main;
		obliqueCamera = true;
		Apply();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			obliqueCamera = !obliqueCamera;
			if (obliqueCamera)
				Apply();
			else
				Reset();
			print(Camera.main.projectionMatrix);
		}
	}

	public void Apply()
	{
		cam.orthographic = true;
		var orthoHeight = cam.orthographicSize;
		var orthoWidth = cam.aspect * orthoHeight;
		var m = Matrix4x4.Ortho(-orthoWidth, orthoWidth, -orthoHeight, orthoHeight, cam.nearClipPlane, cam.farClipPlane);
		var s = zScale / orthoHeight;
		m[0, 2] = +s * Mathf.Sin(Mathf.Deg2Rad * -angle);
		m[1, 2] = -s * Mathf.Cos(Mathf.Deg2Rad * -angle);
		m[0, 3] = -zOffset * m[0, 2];
		m[1, 3] = -zOffset * m[1, 2];
		cam.projectionMatrix = m;
	}
	public void Reset()
	{
		cam.ResetProjectionMatrix();
	}
}
